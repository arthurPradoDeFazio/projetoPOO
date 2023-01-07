using System;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using static AdaCredit.Funcionario;

namespace AdaCredit
{
	public static class ServicosTransacao
	{
		public static void ProcessaTransacoes(string nomeDoArquivo)
		{
            string transacoesPendentes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Transactions", "Pending");
            foreach (string arquivoDeTransferencias in Directory.GetFiles(transacoesPendentes))
                ProcessaTransacao(arquivoDeTransferencias);
		}

        private static void ProcessaTransacao(string arquivoDeTransferencias)
        {
            HashSet<Transferencia> transferenciasValidas = new(), transferenciasInvalidas = new();
            DateOnly dataDasTransferencias = DataDasTransferencias(arquivoDeTransferencias);
            foreach (Transferencia transferenciaPendente in TransferenciasNoArquivo(arquivoDeTransferencias))
            {
                if (transferenciaPendente.TransferenciaValida(dataDasTransferencias))
                    transferenciasValidas.Add(transferenciaPendente);
                else
                    transferenciasInvalidas.Add(transferenciaPendente);
            }

            string arquivoTransferenciasValidas = CaminhoArquivo(arquivoDeTransferencias, true);
            SalvaTransferencias(transferenciasValidas, arquivoTransferenciasValidas);

            string arquivoTransferenciasInvalidas = CaminhoArquivo(arquivoDeTransferencias, false);
            SalvaTransferencias(transferenciasInvalidas, arquivoTransferenciasInvalidas);

            File.Move(arquivoDeTransferencias, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                            "Transactions",
                                                            "Processed",
                                                            Path.GetFileName(arquivoDeTransferencias)));
        }

        private static void SalvaTransferencias(HashSet<Transferencia> transferencias, string arquivoTransferencias)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            

            using (var escritor = new StreamWriter(arquivoTransferencias))
            using (var csv = new CsvHelper.CsvWriter(escritor, config))
            {
                csv.Context.RegisterClassMap<Transferencia.TransferenciaMap>();
                csv.WriteRecords(transferencias);
            }
        }

        private static string CaminhoArquivo(string arquivoDeTransferencias, bool transferenciasValidas)
        {
            string nomeDoArquivo = Path.GetFileName(arquivoDeTransferencias)[..^4];
            string diretorioTransactions = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Transactions");
            string completedOuFailed;
            if (transferenciasValidas)
                completedOuFailed = "-completed.csv";
            else
                completedOuFailed = "-failed.csv";
            return Path.Combine(diretorioTransactions, nomeDoArquivo + completedOuFailed);
        }

        private static DateOnly DataDasTransferencias(string arquivoDeTransferencias)
        {
            string dataDoArquivo = arquivoDeTransferencias[^8..];
            int ano = Convert.ToInt32(dataDoArquivo[0..4]);
            int mes = Convert.ToInt32(dataDoArquivo[4..6]);
            int dia = Convert.ToInt32(dataDoArquivo[6..]);
            return new DateOnly(ano, mes, dia);
        }

        public static HashSet<Transferencia> TransferenciasNoArquivo(string caminhoTransacoes)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };

            IEnumerable<Transferencia> transferencias;
            using (var leitor = new StreamReader(caminhoTransacoes))
            using (var csv = new CsvHelper.CsvReader(leitor, config))
            {
                csv.Context.RegisterClassMap<Transferencia.TransferenciaMap>();
                transferencias = csv.GetRecords<Transferencia>();
            }

            HashSet<Transferencia> transferenciasSet = new();
            foreach (Transferencia t in transferencias)
                transferenciasSet.Add(t);
            return transferenciasSet;
        }
    }
}

