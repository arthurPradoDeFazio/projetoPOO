using System;
using ConsoleTools;

namespace AdaCredit
{
	public partial class AdaCreditController
	{
        private EstadoDeMenu? menuAtual;
        private EstadoDeMenu? telaLogin;
        private EstadoDeMenu? areaDoCliente;
        private EstadoDeMenu? areaDoFuncionario;
        private EstadoDeMenu? areaDeRelatorios;

        
        private void InitMenus(string[] args)
        {
            InitTelaLogin(args);
            areaDeRelatorios = new EstadoDeMenu(new ConsoleMenu(args, 2)
                                                        .Add("Exibir clientes ativos e saldos", () => { })
                                                        .Add("Exibir clientes inativos", () => { })
                                                        .Add("Desativar cadastro de funcionário", () => { })
                                                        .Configure(config =>
                                                        {
                                                            config.Selector = ">> ";
                                                            config.EnableFilter = true;
                                                            config.Title = "Tela principal";
                                                            config.EnableBreadcrumb = true;
                                                            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                        }));
        }

        private void InitTelaLogin(string[] args)
        {
            telaLogin = new EstadoDeMenu(new ConsoleMenu(args, 0)
                                                            .Add("Entre com qualquer tecla para fazer login: ", () => { })
                                                            .Configure(config =>
                                                            {
                                                                config.Selector = ">> ";
                                                                config.EnableFilter = true;
                                                                config.Title = "Login";
                                                                config.EnableBreadcrumb = true;
                                                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                            }));
            
            telaLogin.AdicionaEntrada("Entre com qualquer tecla para fazer login: ", InitTelaPrincipal(args));
        }

        private static EstadoDeMenu InitTelaPrincipal(string[] args)
        {
            EstadoDeMenu telaPrincipal = new(new ConsoleMenu(args, 1)
                                                    .Add("Área do cliente", () => { })
                                                    .Add("Área do funcionário", () => { })
                                                    .Add("Processar de transações", () => { })
                                                    .Add("Gerar relatório", () => { })
                                                    .Configure(config =>
                                                    {
                                                        config.Selector = ">> ";
                                                        config.EnableFilter = true;
                                                        config.Title = "Tela principal";
                                                        config.EnableBreadcrumb = true;
                                                        config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                    }));

            telaPrincipal.AdicionaEntrada("Área do cliente", InitAreaDoClinte(args));
            telaPrincipal.AdicionaEntrada("Área do funcionário", InitAreaDoFuncionario(args));
            telaPrincipal.AdicionaEntrada("Processar de transições", InitAreaDeRelatorios(args));
            telaPrincipal.AdicionaEntrada("Gerar relatório de transições", InitAreaDeRelatorios(args));

            return telaPrincipal;
        }


        private static EstadoDeMenu InitAreaDoClinte(string[] args)
        {
            return new(new ConsoleMenu(args, 2)
                            .Add("Cadastrar novo cliente", () => { })
                            .Add("Consultar dados de cliente", () => { })
                            .Add("Alterar cadastro de cliente", () => { })
                            .Add("Desativar cadastro de cliente", () => { })
                            .Configure(config =>
                            {
                                config.Selector = ">> ";
                                config.EnableFilter = true;
                                config.Title = "Tela principal";
                                config.EnableBreadcrumb = true;
                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                            }));
        }

        private static EstadoDeMenu InitAreaDoFuncionario(string[] args)
        {
            return new EstadoDeMenu(new ConsoleMenu(args, 2)
                                            .Add("Cadastrar novo funcionáro", () => { })
                                            .Add("Alterar senha", () => { })
                                            .Add("Desativar cadastro de funcionário", () => { })
                                            .Configure(config =>
                                            {
                                                config.Selector = ">> ";
                                                config.EnableFilter = true;
                                                config.Title = "Tela principal";
                                                config.EnableBreadcrumb = true;
                                                config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                            }));
        }

        private static EstadoDeMenu InitAreaDeRelatorios(string[] args)
        {
            return new EstadoDeMenu(new ConsoleMenu(args, 2)
                                                        .Add("Exibir clientes ativos e saldos", () => { })
                                                        .Add("Exibir clientes inativos", () => { })
                                                        .Add("Desativar cadastro de funcionário", () => { })
                                                        .Configure(config =>
                                                        {
                                                            config.Selector = ">> ";
                                                            config.EnableFilter = true;
                                                            config.Title = "Tela principal";
                                                            config.EnableBreadcrumb = true;
                                                            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                                                        }));
        }
    }
}

