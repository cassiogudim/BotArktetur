using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Models
{
    [Serializable]
    public class BotBody
    {
        //public BotBody()
        //{
        //    CarrosselMenu.LerArquivoJsonBot();
        //}
        public Dialogs Dialogs { get; set; }
    }

    [Serializable]
    public class Dialogs
    {
        public Sobre Sobre { get; set; }
        public Servicos Servicos { get; set; }
        public Clientes Clientes { get; set; }
        public Fundadores Fundadores { get; set; }
        public Parceiros Parceiros { get; set; }
        public Contatos Contatos { get; set; }
    }
    [Serializable]
    public class Sobre
    {
        public string NomeBot { get; set; }
        public string NomeEmpresa { get; set; }
        public string SobreEmpresa { get; set; }
        public string OpcoesVoltar { get; set; }
    }
    [Serializable]
    public class Servicos
    {
        public int Quantidade { get; set; }
        public string TextoServico { get; set; }
        public List<Servico> ListaServico { get; set; }
    }
    [Serializable]
    public class Servico
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
    [Serializable]
    public class Clientes
    {
        public int Quantidade { get; set; }
        public string TextoCliente { get; set; }
        public List<Cliente> ListaCliente { get; set; }
    }
    [Serializable]
    public class Cliente
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
    [Serializable]
    public class Fundadores
    {
        public int Quantidade { get; set; }
        public string TextoFundador { get; set; }
        public List<Fundador> ListaFundador { get; set; }
    }
    [Serializable]
    public class Fundador
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
    [Serializable]
    public class Parceiros
    {
        public int Quantidade { get; set; }
        public string TextoParceiro { get; set; }
        public List<Parceiro> ListaParceiro { get; set; }
    }
    [Serializable]
    public class Parceiro
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string Site { get; set; }
    }
    [Serializable]
    public class Contatos
    {
        public int Quantidade { get; set; }
        public string TextoContato { get; set; }
        public List<Contato> ListaContato { get; set; }
    }
    [Serializable]
    public class Contato
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string UrlRedirecionamento { get; set; }
    }
}