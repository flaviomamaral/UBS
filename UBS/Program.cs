using System.Runtime.InteropServices;
using UBS;

//Necessario alterar tipo de chamada para assinrona e alterar os metodos nas classes Service, FIX e RuleEngine para async
//Comentarios sobre o codigo estao nas classes FIX e RuleEngine

var svc = new Service();
var task = Task.Run(() => svc.Run());
Task.WaitAll(new Task[] { task });


//[Route("v1/users/")]
//[HttpGet]
//public IHttpActionResult GetUser([FromURI] int id)
//{
//    UserService USER_Service = new UserService();
//    return USER_Service.GetUser(id);
//}

//Neste caso eu criaria uma camada de negocio contendo a classe UserBusiness, fazendo a referencia para a camada de Serviços,
//pois o controller não é responsável por instanciar diretamente um metodo que retorna dados sem tratamento.
//Outro problema é que o tipo Int caso nao receba nenhum valor como parametro assumira o valor zero,
//o que pode retornar um objeto nulo do serviço, que não encontrará nenhum registro com este id.
//Precisa adicionar um tratamento para verificar se o id é maior que zero, e outro para tratar o retorno quando o usuario nao é encontrado.
//Pode criar um objeto (model) para o retorno e tambem utilizar um try catch para retornar OK(model) (HTTP 200) ou Error() HTTP 500
