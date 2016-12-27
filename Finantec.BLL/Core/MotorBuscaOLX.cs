using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Finantec.Modelo.Entidades;
using System.Net;

namespace Finantec.BLL.Core
{
    public class MotorBuscaOLX
    {
        private string _urlBase = "";

        int maxItemPorPagina = 32;
        decimal totalpagina = 1;



        public string PalavraChave { get; set; }
        public int PaginaCorrente { get; set; }
        public string Categoria { get; private set; }

        public enum Regiao
        {
            SaoPaulo = 1
        }

        Dictionary<Regiao, string> dic = new Dictionary<Regiao, string>();


        public MotorBuscaOLX()
        {
            this.configurar();
            this.PaginaCorrente = 1;
        }

        private void configurar()
        {
            this.dic.Add(Regiao.SaoPaulo, "http://sp.olx.com.br");
            this._urlBase = this.dic[Regiao.SaoPaulo];
        }

        public MotorBuscaOLX(Regiao regiao)
        {
            this.configurar();
            this._urlBase = this.dic[regiao];
        }




        public Pesquisa BuscarAnuncioDaLista(string palavraChave, int? numeroPagina = null)
        {

            //string url = "http://sp.olx.com.br/sao-paulo-e-regiao/zona-sul/videogames";

            this.PalavraChave = palavraChave;
            this.Categoria = "videogames";

            if (numeroPagina.HasValue)
            {
                this.PaginaCorrente = numeroPagina.Value;
            }



            string url = this.configurarUrlPesquisa(this._urlBase, "/sao-paulo-e-regiao/zona-sul/", this.Categoria, this.PalavraChave, PaginaCorrente);



            HtmlWeb navegadorWeb = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.GetEncoding("iso-8859-9")
            };


            HtmlDocument documentoHTML = navegadorWeb.Load(url);

            ////*[@id="main-ad-list"]
            HtmlNode ul_ProdutosAnuncioados = documentoHTML.DocumentNode.SelectSingleNode("//*[@id='main-ad-list']");


            ////*[@id="main-ad-list"]/li[1]

            HtmlNode[] li_anuncios = ul_ProdutosAnuncioados.SelectNodes("li").ToArray() ?? null;

            List<Anuncio> listaAnuncio = new List<Anuncio>();

            HtmlNode divFiltro = documentoHTML.DocumentNode.SelectSingleNode("//div[@class='section_filter-order']");

            var pesquisa = new Pesquisa();

            pesquisa.Descricao = "Busca do produto " + palavraChave;

            if (divFiltro != null)
            {
                var nodes = divFiltro.Descendants("ul");
                HtmlNode liResultado = nodes.FirstOrDefault().Descendants("span").FirstOrDefault(node => node.Attributes.Any(a => a.Value == "qtd"));
                if (liResultado != null)
                {
                    decimal _totalItemAnuncio;
                    if (decimal.TryParse(liResultado.InnerText.Trim(), out _totalItemAnuncio))
                    {
                        pesquisa.TotalAnuncio = _totalItemAnuncio;
                        pesquisa.TotalPagina = Math.Floor((_totalItemAnuncio / this.maxItemPorPagina));
                        this.totalpagina = pesquisa.TotalPagina > 0 ? pesquisa.TotalPagina : 1;
                    }
                }

            }



            List<Anuncio> auxListaAnuncio = new List<Anuncio>();

            for (int i = 1; i <= this.totalpagina; i++)
            {
                this.PaginaCorrente = i;

                if (this.PaginaCorrente == 1)
                {
                    //auxListaAnuncio.AddRange(this.extrairAnuncio(li_anuncios));
                    this.extrairAnuncio(li_anuncios, ref auxListaAnuncio);
                }
                else
                {



                    url = this.configurarUrlPesquisa(this._urlBase, "/sao-paulo-e-regiao/zona-sul/", this.Categoria, this.PalavraChave, PaginaCorrente);
                    documentoHTML = navegadorWeb.Load(url);
                    ////*[@id="main-ad-list"]
                    ul_ProdutosAnuncioados = documentoHTML.DocumentNode.SelectSingleNode("//*[@id='main-ad-list']");
                    li_anuncios = ul_ProdutosAnuncioados.SelectNodes("li").ToArray() ?? null;

                    if (li_anuncios != null)
                        this.extrairAnuncio(li_anuncios, ref auxListaAnuncio);
                    //auxListaAnuncio.AddRange(this.extrairAnuncio(li_anuncios));


                }


            }



            pesquisa.Anuncios = auxListaAnuncio;

            return pesquisa;
        }

        private string configurarUrlPesquisa(string _urlBase, string regiaoBusca, string categoria, string palavraChave, int paginaCorrente)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(_urlBase);
            sbUrl.Append(regiaoBusca);
            sbUrl.Append(this.Categoria);
            sbUrl.AppendFormat("?o={0}", paginaCorrente);
            sbUrl.AppendFormat("&q={0}", palavraChave.Replace(" ", "+"));

            return sbUrl.ToString();
        }

        private void extrairAnuncio(HtmlNode[] li_anuncios, ref List<Anuncio> listaAnuncio)
        {
            Anuncio anuncio;
            //List<Anuncio> listaAnuncio = new List<Anuncio>();

            foreach (HtmlNode item in li_anuncios)
            {

                try
                {
                    anuncio = new Anuncio();


                    HtmlNode a_nodeAnuncio = item.Descendants("a").FirstOrDefault(node => node.Attributes.Any(a => a.Value == "OLXad-list-link"));
                    string urlAnuncio = a_nodeAnuncio.Attributes["href"].Value ?? "";
                    string codigoAnuncio = a_nodeAnuncio.Attributes["name"].Value ?? a_nodeAnuncio.Attributes["id"].Value ?? "";

                    anuncio.Url = urlAnuncio.Split('?').FirstOrDefault();
                    anuncio.CodigoAnuncio = codigoAnuncio;
                    anuncio.Produto = new Produto();

                    //obter o titulo
                    //*[@id="284757683"]/div[2]/div[1]/h3
                    string titulo = item.Descendants("h3").FirstOrDefault().InnerText ?? "";

                    anuncio.Titulo = titulo.TrimStart().TrimEnd().Trim();
                    anuncio.Produto.Nome = anuncio.Titulo;

                    anuncio.DataPublicacao = DateTime.Now;


                    //foto do produto
                    string urlFoto = item.Descendants("img").FirstOrDefault().Attributes["src"].Value ?? "";
                    anuncio.Produto.FotosProduto.Add(new FotoProduto() { Url = urlFoto });

                    anuncio.Localizacao = new Endereco();

                    //endere
                    string informacaoEndereco = item.Descendants("p").FirstOrDefault(node => node.Attributes.Any(a => a.Value == "text detail-region")).InnerText ?? "";
                    if (!string.IsNullOrEmpty(informacaoEndereco) && informacaoEndereco.Split(',').Count() > 1)
                    {
                        var arrEndereco = informacaoEndereco.Split(',');
                        anuncio.Localizacao.Municipio = arrEndereco[0].Trim();
                        anuncio.Localizacao.Bairro = arrEndereco[1].Trim();

                    }

                    string preco = item.Descendants("p").FirstOrDefault(node => node.Attributes.Any(a => a.Value == "OLXad-list-price")).InnerText ?? "";
                    decimal _preco;

                    if (decimal.TryParse(preco.Trim().Replace("R$", ""), out _preco))
                    {
                        anuncio.Produto.Preco = _preco;
                    }

                    //data de publicação

                    var listInformacaoPublicacao = item.SelectSingleNode("a").SelectSingleNode("div[@class='col-4']").SelectNodes("p").ToList() ?? null;

                    if (listInformacaoPublicacao != null && listInformacaoPublicacao.Count > 0)
                    {
                        string[] _dataPublicacao = listInformacaoPublicacao.FirstOrDefault().InnerText.Split(' ');

                        string[] horaPublicacao = listInformacaoPublicacao.LastOrDefault().InnerText.Split(':');
                        int dia, mes, hora, minuto;
                        hora = Convert.ToInt32(horaPublicacao[0]);
                        minuto = Convert.ToInt32(horaPublicacao[1]);


                        if (_dataPublicacao.Count() > 1)
                        {
                            int.TryParse(_dataPublicacao[0], out dia);
                            mes = this.obterNumeroMes(_dataPublicacao[1]);


                            anuncio.DataPublicacao = new DateTime(DateTime.Now.Year, mes, dia, hora, minuto, 0);

                        }
                        else if (_dataPublicacao[0].ToLower().Contains("hoje"))
                        {
                            anuncio.DataPublicacao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hora, minuto, 0);
                        }
                    }

                    listaAnuncio.Add(anuncio);
                    //Console.WriteLine(item.InnerHtml);
                }
                catch (Exception ex)
                {

                    continue;
                }


            }
            //return listaAnuncio;
        }

        private int obterNumeroMes(string nomeMes)
        {

            if (nomeMes.ToLower().Contains("dez"))
                return 12;

            if (nomeMes.ToLower().Contains("nov"))
                return 11;

            if (nomeMes.ToLower().Contains("out"))
                return 10;

            if (nomeMes.ToLower().Contains("set"))
                return 9;

            if (nomeMes.ToLower().Contains("ago"))
                return 8;

            if (nomeMes.ToLower().Contains("jul"))
                return 7;

            if (nomeMes.ToLower().Contains("jun"))
                return 6;

            if (nomeMes.ToLower().Contains("mai"))
                return 5;

            if (nomeMes.ToLower().Contains("abr"))
                return 4;

            if (nomeMes.ToLower().Contains("mar"))
                return 3;

            if (nomeMes.ToLower().Contains("fev"))
                return 2;
            if (nomeMes.ToLower().Contains("jan"))
                return 1;

            return DateTime.Now.Month;
        }
    }
}
