using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
	public class Cliente
	{
		public string Nome { get; init; }
		public string Sobrenome { get; init; }
		public string Senha { get; init; }
		public string Conta { get; init; }
		public char DigitoVerficador { get; init; }
		public decimal Saldo { get; init; }
		public string Agencia { get; init; }
		public bool Ativo { get; init; }

		public class ClienteMap : ClassMap<Cliente>
		{
			public ClienteMap()
			{
				Map(c => c.Nome).Index(1).Name("nome");
				Map(c => c.Sobrenome).Index(5).Name("sobrenome");
				Map(c => c.Senha).Index(6).Name("senha");
				Map(c => c.Conta).Index(0).Name("conta");
				Map(c => c.DigitoVerficador).Index(4).Name("digito");
				Map(c => c.Saldo).Index(2).Name("saldo");
				Map(c => c.Agencia).Index(3).Name("agencia");
				Map(c => c.Ativo).Index(7).Name("atividade");
			}
		}

		public static Dictionary<string, Cliente> ClientesNoArquivo()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				Delimiter = ";"
			};

			IEnumerable<Cliente> clientes;
			string nomeDoArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Clientes.csv");
			using (var leitor = new StreamReader(nomeDoArquivo))
			using (var csv = new CsvHelper.CsvReader(leitor, config))
			{
                csv.Context.RegisterClassMap<ClienteMap>();
                clientes = csv.GetRecords<Cliente>().ToList();
			}

			Dictionary<string, Cliente> contaCliente = new();
			foreach (Cliente c in clientes)
				contaCliente.Add(c.Conta, c);
			return contaCliente;
		}

		public static void VerClientes(string senha)
		{
			foreach (var c in ClientesNoArquivo())
				Console.WriteLine(c.Value);
		}

		public static char CalculaDigito(string conta)
		{
            int d = 0, mult = 2;
            foreach (char c in conta.Reverse())
            {
                d += (c - '0') * mult;
                mult += 1;
            }

			if (d % 11 == 0 || d % 11 == 1)
				return '0';
            return (char)((11 - (d % 11)) + '0');
        }

		public void SalveNoCSV(string nomeDoArquivo)
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				Delimiter = ";"
			};

			using (var stream = File.Open(nomeDoArquivo, FileMode.Append))
			using (var escritor = new StreamWriter(stream))
			using (var csv = new CsvWriter(escritor, config))
			{
				csv.Context.RegisterClassMap<ClienteMap>();
				csv.WriteRecord(this);
			}
        }

		public override string ToString()
		{
			return $"\tNome: {Nome} {Sobrenome}{Environment.NewLine}\tSaldo: {Saldo}{Environment.NewLine}\tAtividade: {(Ativo ? "ativo" : "inativo")}";
		}
    }

}

