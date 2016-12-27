using Finantec.BLL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finantec.ProdutoInteresse
{
    class Program
    {
        static void Main(string[] args)
        {
            var motor = new MotorBuscaOLX();

            Console.WriteLine("Informe uma palavra para busca");
            var palavraChave = Console.ReadLine();
            var pesquisa = motor.BuscarAnuncioDaLista(palavraChave);

            Console.WriteLine("O total de anuncio é: {0}", pesquisa.TotalAnuncio);

            foreach (var anuncio in pesquisa.Anuncios)
            {
                Console.WriteLine(string.Format("{0} - Preco: {1}", anuncio.Titulo, anuncio.Produto.Preco.ToString("c")));
                Console.WriteLine(anuncio.Url);
                Console.WriteLine();
            }

            var pesquisaService = new BLL.Servico.PesquisaService();
            pesquisaService.SalvarPesquisa(pesquisa);

            Console.ReadKey();


        }
    }
}
