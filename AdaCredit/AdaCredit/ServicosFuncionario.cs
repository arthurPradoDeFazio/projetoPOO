using System;
using System.Globalization;
using System.Linq;
using Bogus;
using CsvHelper.Configuration;

namespace AdaCredit
{
	public static class ServicosFuncionario
	{
		public static void CadastrarFuncionario()
		{
			Console.Write("Entre com o primeiro nome do funcionário: ");
			string? nome = Console.ReadLine();
			if (nome == null)
				throw new IOException("Não foi possível ler o primeiro nome do funcionário. Cadastro não realizado"); // TODO criar exceção mais especifica?

			Console.Write("Entre com último nome do funcionário: ");
			string? sobrenome = Console.ReadLine();
			if (sobrenome == null)
				throw new IOException("Não foi possível ler o último nome do funcionário. Cadastro não realizado");

			Dictionary<string, Funcionario> funcionarios = Funcionario.FuncionariosNoArquivo();
			if (funcionarios.Any(c => c.Value.Nome == nome && c.Value.Sobrenome == sobrenome))
				throw new ArgumentException("Funcionário já cadastrado!");

			string senha = ServicosFuncionario.ColetaSenha();

			var novoFuncionario = new Funcionario {
				Nome = nome,
				Sobrenome = sobrenome,
				Senha = senha,
				HoraUltimoLogin = "N/A",
                DataUltimoLogin = "N/A",
				Ativo = true
			};

			novoFuncionario.SalveNoCSV(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Funcionarios.csv"));

		}

        private static Funcionario ColetaFuncionario()
        {
            Console.Write("Entre com o primeiro nome do funcionário: ");
            string? nome = Console.ReadLine();
            if (nome == null)
                throw new IOException("Não foi possível ler o nome do funcionário");

            Console.Write("Entre com o sobrenome do funcionário: ");
            string? sobrenome = Console.ReadLine();
            if (sobrenome == null)
                throw new IOException("Não foi possível ler o nome do funcionário");

            var funcionarios = Funcionario.FuncionariosNoArquivo();
            if (!funcionarios.TryGetValue($"{nome} {sobrenome}", out Funcionario? funcionarioConsultado))
                throw new IOException("Não há cliente com esse número de conta!");

            Console.Write("Entre com a senha do funcionário: ");
            string? senha = Console.ReadLine();
            if (senha == null)
                throw new IOException("Não foi possível ler a senha");
			if (senha != funcionarioConsultado.Senha)
				throw new ArgumentException("Senha incorreta!");

			return funcionarioConsultado;
        }

		public static void DesativarCadastro()
		{
			var funcionario = ColetaFuncionario();
			if (!funcionario.Ativo)
				return;

			var novoFuncionario= new Funcionario
			{
				Nome = funcionario.Nome,
				Sobrenome = funcionario.Sobrenome,
				Senha = funcionario.Senha,
                DataUltimoLogin = funcionario.DataUltimoLogin,
                HoraUltimoLogin = funcionario.HoraUltimoLogin,
				Ativo = false
			};

			var funcionarios = Funcionario.FuncionariosNoArquivo();
			funcionarios[$"{novoFuncionario.Nome} {novoFuncionario.Sobrenome}"] = novoFuncionario;
			ServicosFuncionario.SubstituaArquivo(funcionarios);
		}
				
        public static void AlteraSenha()
        {
            var funcionario = ColetaFuncionario();
            if (!funcionario.Ativo)
                throw new ArgumentException("Funcionário inativo!");

            Console.Write("Entre com a nova senha: ");
            string? novaSenha = Console.ReadLine();
            if (novaSenha== null)
                throw new IOException("Não foi possível ler a nova senha");
            var novoFuncionario = new Funcionario
            {
                Nome = funcionario.Nome,
                Sobrenome = funcionario.Sobrenome,
                Senha = novaSenha,
				DataUltimoLogin = funcionario.DataUltimoLogin,
				HoraUltimoLogin = funcionario.HoraUltimoLogin,
                Ativo = funcionario.Ativo
            };

            var funcionarios = Funcionario.FuncionariosNoArquivo();
            funcionarios[$"{novoFuncionario.Nome} {novoFuncionario.Sobrenome}"] = novoFuncionario;
            ServicosFuncionario.SubstituaArquivo(funcionarios);
        }

        private static void SubstituaArquivo(Dictionary<string, Funcionario> funcionarios)
        {
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            string nomeDoArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Funcionarios.csv");

            using (var leitor = new StreamWriter(nomeDoArquivo))
            using (var csv = new CsvHelper.CsvWriter(leitor, config))
            {
                csv.Context.RegisterClassMap<Funcionario.FuncionarioMap>();
				csv.WriteRecords(funcionarios.Select(entrada => entrada.Value));
            }
		}
		
        private static string ColetaSenha()
        {
            string? senha, confirmacao;
				
			Console.Write("Entre com a sua senha: "); // TODO ocultar senha quando for digitar
			senha = Console.ReadLine();

			Console.Write("Entre com a senha novamente para confirmação: "); // TODO ocultar
			confirmacao = Console.ReadLine();
			
			if (senha == null || confirmacao == null)
				throw new IOException("Não foi possível coletar a senha. Cadastro não realizado");

			while (senha != confirmacao)
			{
				Console.Write("Senhas diferentes! Entre novamente com a confirmação: ");
				confirmacao = Console.ReadLine();
				if (confirmacao == null)
					throw new IOException("Não foi possível coletar a senha. Cadastro não realizado");
			}

			return senha;
        }
    }
}

