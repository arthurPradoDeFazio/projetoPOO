using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit
{
	public class Funcionario
	{
		public string Nome { get; init; }
		public string Sobrenome { get; init; }
		public string Senha { get; init; }
		public string DataUltimoLogin { get; init; }
		public string HoraUltimoLogin { get; init; }
		public bool Ativo { get; init; }

		public class FuncionarioMap : ClassMap<Funcionario>
		{
			public FuncionarioMap()
			{
				Map(c => c.Nome).Index(0).Name("nome");
				Map(c => c.Sobrenome).Index(1).Name("sobrenome");
				Map(c => c.Senha).Index(4).Name("senha");
				Map(c => c.DataUltimoLogin).Index(3).Name("data_ultimo_login");
				Map(c => c.HoraUltimoLogin).Index(2).Name("hora_ultimo_login");
				Map(c => c.Ativo).Index(5).Name("atividade");
			}
		}

		public static Dictionary<string, Funcionario> FuncionariosNoArquivo()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				Delimiter = ";"
			};

			IEnumerable<Funcionario> funcionarios;
			string nomeDoArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Funcionarios.csv");
			using (var leitor = new StreamReader(nomeDoArquivo))
			using (var csv = new CsvHelper.CsvReader(leitor, config))
			{
                csv.Context.RegisterClassMap<FuncionarioMap>();
                funcionarios= csv.GetRecords<Funcionario>().ToList();
			}

			Dictionary<string, Funcionario> nomeFuncionario = new();
			foreach (Funcionario f in funcionarios)
				nomeFuncionario.Add($"{f.Nome} {f.Sobrenome}", f);
			return nomeFuncionario;
		}

		public static void VerClientes(string senha)
		{
			foreach (var f in FuncionariosNoArquivo())
				Console.WriteLine(f.Value);
		}

		public void SalveNoCSV(string nomeDoArquivo)
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
				Delimiter = ";"
			};

			using (var stream = File.Open(nomeDoArquivo, FileMode.Append))
			using (var escritor = new StreamWriter(stream))
			using (var csv = new CsvWriter(escritor, config))
			{
				csv.Context.RegisterClassMap<FuncionarioMap>();
				csv.WriteRecords(new List<Funcionario> { this });
			}
        }

		public override string ToString()
		{
			return $"\tNome: {Nome} {Sobrenome}{Environment.NewLine}\tÚltimo login: {DataUltimoLogin} às {HoraUltimoLogin}{Environment.NewLine}\tAtividade: {(Ativo ? "ativo" : "inativo")}";
		}
    }

}

