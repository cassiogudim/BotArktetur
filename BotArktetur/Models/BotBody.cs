using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotArktetur.Models
{
    public class BotBody
    {
        public Dialogs Dialogs { get; set; }
    }

    public class Dialogs
    {
        public Sobre Sobre { get; set; }
        public Servicos Servicos { get; set; }
        public Clientes Clientes { get; set; }
        public Fundadores Fundadores { get; set; }
        public Parceiros Parceiros { get; set; }
        public Contatos Contatos { get; set; }
    }

    public class Sobre
    {
        public string NomeBot { get; set; }
        public string SobreEmpresa { get; set; }
    }

    public class Servicos
    {
        public int Quantidade { get; set; }
        public List<Servico> ListaServico { get; set; }
    }

    public class Servico
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    public class Clientes
    {
        public int Quantidade { get; set; }
        public List<Cliente> ListaCliente { get; set; }
    }

    public class Cliente
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }

    public class Fundadores
    {
        public int Quantidade { get; set; }
        public List<Fundador> ListaFundador { get; set; }
    }

    public class Fundador
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }

    public class Parceiros
    {
        public int Quantidade { get; set; }
        public List<Parceiro> ListaParceiro { get; set; }
    }

    public class Parceiro
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }

    public class Contatos
    {
        public int Quantidade { get; set; }
        public List<Contato> ListaContato { get; set; }
    }

    public class Contato
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string UrlRedirecionamento { get; set; }
    }
}