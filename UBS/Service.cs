namespace UBS
{
    public class Service
    {
        public void Run()
        {
            var engine = new RuleEngine();
            var fix = new FIX();

            while (true)
            {
                engine.Execute();
                fix.Execute();
            }
        }
    }
}
