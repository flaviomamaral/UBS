using System.Data.SqlClient;

namespace UBS
{
    public class RuleEngine : IRuleEngine
    {
        public void Execute()
        {
            //Necessario adotar uma solução para executar periodicamente (a cada 1 hora por exemplo), seja via codigo ou via agendamento no servidor.
            //A opção mais segura é via codigo pois nao permite erro humano de configuração do agendamento no servidor.
            //Esta solução evita que sejam gerados milhares de arquivos, e limita o numero de arquivos diarios que sao criados no servidor
            var connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projetos\\Teste\\UBS\\UBS\\mainDb.mdf;Integrated Security=True";
            var query = $"SELECT evento FROM eventos WHERE timestamp > '{DateTime.Now.AddMinutes(-120):yyyy-MM-dd hh:mm:ss}'"; //Modificar esta condição para se adequar às novas definições (Retirar limite por tempo e adicionar condição para trazer apenas eventos nao lidos)
            var conn = new SqlConnection(connString);
            var cmd = new SqlCommand(query, conn);
            // A CONEXÃO É ABERTA MAS NÃO ESTÁ SENDO FECHADA, EU UTILIZARIA UM BLOCO USING (POR PADRAO NAS APLICAÇÕES) PARA EVITAR QUE SE ESQUEÇA DE FECHAR A CONEXÃO.
            //using (conn) { ... }
            conn.Open();
            var reader = cmd.ExecuteReader();
            var eventos = new List<string>();
            while (reader.Read()) //Este loop infinito gera arquivos duplicados mesmo que não haja novos eventos (Pode ser verificado o resultado na pasta TEMP).
            {
                eventos.Add(reader[0].ToString());
                //Necessario adicionar um campo bit não nulo com valor padrao 0 (zero) na tabela eventos e atualizar a query pra trazer apenas os eventos que ainda não foram lidos (novocampobit = 0).
                //Fazer o update do registro para alterar o novo campo bit para valor 1 (Um), evitando que ele seja novamente listado no arquivo txt.
            }
            conn.Close(); //Fechamento de conexão adicionada como medida para correção imediata da causa raiz do problema.
            File.WriteAllLines($@"C:\Projetos\Teste\UBS\TEMP\eventos_{Guid.NewGuid()}.txt", eventos.ToArray());
        }
    }
}
