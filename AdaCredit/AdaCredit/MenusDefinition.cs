using System;
using ConsoleTools;

namespace AdaCredit
{
	public partial class AdaCreditController
	{
        private EstadoDeMenu? menuAtual;
        private EstadoDeMenu? telaLogin;
        private EstadoDeMenu? telaPrincipal;
        
        private void InitMenus(string[] args)
        {
            menuAtual = InitTelaLogin(args);           
        }

        private EstadoDeMenu InitTelaLogin(string[] args)
        {
            telaLogin = new EstadoDeMenu(new ConsoleMenu(args, 0)
                                                            .Add("Entre com qualquer tecla para fazer login: ", ConsoleMenu.Close)
                                                            .Configure(config =>
                                                            {
                                                                config.Selector = ">> ";
                                                                config.EnableFilter = true;
                                                                config.Title = "Login";
                                                                config.EnableBreadcrumb = true;
                                                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                            })); 
            InitTelaPrincipal(args, telaLogin);
            telaLogin.AdicionaEntrada("Entre com qualquer tecla para fazer login: ", telaPrincipal);

            return telaLogin;
        }

        private EstadoDeMenu InitTelaPrincipal(string[] args, EstadoDeMenu anterior)
        {
            telaPrincipal = new(new ConsoleMenu(args, 1)
                                        .Add("Área do cliente", ConsoleMenu.Close)
                                        .Add("Área do funcionário", ConsoleMenu.Close)
                                        .Add("Processar de transações", ConsoleMenu.Close)
                                        .Add("Gerar relatório", ConsoleMenu.Close)
                                        .Add("Voltar", ConsoleMenu.Close)
                                        .Configure(config =>
                                        {
                                            config.Selector = ">> ";
                                            config.EnableFilter = true;
                                            config.Title = "Tela principal";
                                            config.EnableBreadcrumb = true;
                                            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                        }));

            telaPrincipal.AdicionaEntrada("Área do cliente", InitAreaDoClinte(args, telaPrincipal));
            telaPrincipal.AdicionaEntrada("Área do funcionário", InitAreaDoFuncionario(args, telaPrincipal));
            telaPrincipal.AdicionaEntrada("Gerar relatório", InitAreaDeRelatorios(args, telaPrincipal));
            telaPrincipal.AdicionaEntrada("Voltar", anterior);

            return telaPrincipal;
        }


        private static EstadoDeMenu InitAreaDoClinte(string[] args, EstadoDeMenu anterior) {
            EstadoDeMenu areaDoCliente = new(new ConsoleMenu(args, 2)
                            .Add("Cadastrar novo cliente", ConsoleMenu.Close)
                            .Add("Consultar dados de cliente", ConsoleMenu.Close)
                            .Add("Alterar cadastro de cliente", ConsoleMenu.Close)
                            .Add("Desativar cadastro de cliente", ConsoleMenu.Close)
                            .Add("Voltar", ConsoleMenu.Close)
                            .Configure(config =>
                            {
                                config.Selector = ">> ";
                                config.EnableFilter = true;
                                config.Title = "Tela principal";
                                config.EnableBreadcrumb = true;
                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                            }));

            areaDoCliente.AdicionaEntrada("Voltar", anterior);

            return areaDoCliente;
        }

        private static EstadoDeMenu InitAreaDoFuncionario(string[] args, EstadoDeMenu anterior)
        {
            EstadoDeMenu areaDoFuncionario = new(new ConsoleMenu(args, 2)
                                            .Add("Cadastrar novo funcionáro", ConsoleMenu.Close)
                                            .Add("Alterar senha", ConsoleMenu.Close)
                                            .Add("Desativar cadastro de funcionário", ConsoleMenu.Close)
                                            .Add("Voltar", ConsoleMenu.Close)
                                            .Configure(config =>
                                            {
                                                config.Selector = ">> ";
                                                config.EnableFilter = true;
                                                config.Title = "Tela principal";
                                                config.EnableBreadcrumb = true;
                                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                            }));

            areaDoFuncionario.AdicionaEntrada("Voltar", anterior);

            return areaDoFuncionario;
        }

        private static EstadoDeMenu InitAreaDeRelatorios(string[] args, EstadoDeMenu anterior)
        { 
            EstadoDeMenu areaDeRelatorios = new(new ConsoleMenu(args, 2)
                                                    .Add("Exibir clientes ativos e saldos", ConsoleMenu.Close)
                                                    .Add("Exibir clientes inativos", ConsoleMenu.Close)
                                                    .Add("Desativar cadastro de funcionário", ConsoleMenu.Close)
                                                    .Add("Voltar", ConsoleMenu.Close)
                                                    .Configure(config =>
                                                    {
                                                        config.Selector = ">> ";
                                                        config.EnableFilter = true;
                                                        config.Title = "Tela principal";
                                                        config.EnableBreadcrumb = true;
                                                        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                    }));

            areaDeRelatorios.AdicionaEntrada("Voltar", anterior);

            return areaDeRelatorios;
        }
    }
}

