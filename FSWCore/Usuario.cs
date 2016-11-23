using SQLite;
using System;

[Serializable()]
public class Usuario
{
    public Usuario()
    {

    }

    public Usuario(string cpf, string nome, string sobrenome, string senha)
    {
        this.CPF = cpf;
        this.Nome = nome;
        this.Senha = senha;
        this.DataNascimento = new DateTime();
    }

    [PrimaryKey, Unique, NotNull, MaxLength(11)]
    public string CPF { get; set; }

    [NotNull]
    public string Nome { get; set; }

    [NotNull]
    public string Senha { get; set; }

    public DateTime DataNascimento { get; set; }

    public string toString()
    {
        return string.Format("["
            + this.GetType().Name + ": "
            + "CPF={0}, "
            + "Nome={1}, "
            + "DataNascimento={2}"
            + "]",
            CPF, Nome, DataNascimento.Date.ToShortDateString());
    }
}