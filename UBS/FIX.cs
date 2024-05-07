namespace UBS
{
    public class FIX : IFIX
    {
        public void Execute()
        {
            var timeBuckets = GetTimeBucketDictionary();
            var filePath = @"C:\Projetos\Teste\UBS\TEMP\fix_session.summary";

            var lines = File.ReadAllLines(filePath)
                .Where(msg => msg.StartsWith(" IN") && msg.Contains("35=D"))
                .Select(msg => msg.Substring(13, 8))
                .GroupBy(t => t)
                .Select(x => (chave: x.Key, valor: x.Count()));

            foreach (var line in lines)
            {
                timeBuckets[line.chave] = line.valor;
            }

            SaveCSV(timeBuckets, @"C:\Projetos\Teste\UBS\TEMP\output.csv");
        }

        public Dictionary<string, int> GetTimeBucketDictionary()
        {

            var dict = new Dictionary<string, int>();
            var startingDate = new DateTime(2021, 02, 22, 10, 00, 00);
            var targetTime = new DateTime(2021, 02, 22, 21, 00, 00);

            //percorre o total de segundos entre o startingDate e o targetTime
            for (DateTime date = startingDate; date <= targetTime; date = date.AddSeconds(1))
            {
                dict.Add(date.TimeOfDay.ToString(), 0);
                //adiciona o intervalo do tempo no dicionario
            }

            return dict;
            //retorna uma lista com todos os intervalos por hora entre o startingDate e o targetTime
        }

        public void SaveCSV(Dictionary<string, int> dict, string outputFilePath)
        {
            var file = dict.Select(x => $"{x.Key},{x.Value}").ToArray();
            File.WriteAllLines(outputFilePath, file);

            //39.600 linhas é o equivalente ao numero de segundos entre 10h (startingDate) e 21h (targetTime)
            //Cada hora passada sao adicionadas 3.600 linhas, adicionando 1 segundo em cada linha
        }
    }
}
