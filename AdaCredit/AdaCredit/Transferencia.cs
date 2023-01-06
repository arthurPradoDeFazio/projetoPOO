using System;
namespace AdaCredit
{
	public class Transferencia
	{
		public string CodigoDoBancoDeOrigem { get; init; }
		public string AgenciaDoBancoDeOrigem { get; init; }
		public string ContaDeOrigem { get; init; }

		public string CodigoDoBancoDeDestino { get; init; }
        public string AgenciaDoBancoDeDestino { get; init; }
        public string ContaDeDestino { get; init; }

		public string Transacao { get; init; }

		public decimal ValorTransferencia { get; init; }


		public bool TipoDeTransicaoValido() =>
			(Transacao == "TEF" || Transacao == "DOC" || Transacao == "TED") && !(Transacao == "TEF" && CodigoDoBancoDeOrigem != CodigoDoBancoDeDestino);

		public decimal Tarifa(DateOnly dataDaTransacao)
		{
			if (Transacao == "TEF" || dataDaTransacao.CompareTo(new DateOnly(2022, 11, 20)) <= 0)
				return 0;
			if (Transacao == "TED")
				return 5;
			return 1 + Math.Max(0.01M * ValorTransferencia, 5);
		}

		public bool ContasValidas()
		{
			var clientes = Cliente.ClientesNoArquivo();

			if (CodigoDoBancoDeOrigem == "777" && (AgenciaDoBancoDeOrigem != "0001"
												   || !clientes.TryGetValue(ContaDeOrigem[0..^1], out Cliente? clienteOrigem)
												   || clienteOrigem.DigitoVerficador != ContaDeOrigem[^1]
												   || !clienteOrigem.Ativo))
			{
				return false; // origem inválida
			}

			if (CodigoDoBancoDeDestino == "777" && (AgenciaDoBancoDeDestino != "0001"
												   || !clientes.TryGetValue(ContaDeDestino[0..^1], out Cliente? clienteDestino)
												   || clienteDestino.DigitoVerficador != ContaDeDestino[^1]
												   || !clienteDestino.Ativo))
			{
				return false; // destino inválido
			}

			return true;
		}

		public bool SaldoSuficiente(DateOnly dataDaTransacao)
		{
			if (CodigoDoBancoDeOrigem != "777")
				return true;
			var clienteOrigem = Cliente.ClientesNoArquivo()[ContaDeOrigem[0..^1]];
            return clienteOrigem.Saldo - ValorTransferencia - Tarifa(dataDaTransacao) >= 0;
		}

        public bool TarifaValida(DateOnly dataDaTransacao) => TipoDeTransicaoValido() && ContasValidas() && SaldoSuficiente(dataDaTransacao);
    }
}

