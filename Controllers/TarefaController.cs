using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        [HttpPost("CriarTarefa")]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            if (ModelState.IsValid) //Valida
            {
                _context.Tarefas.Add(tarefa); //Adiciona as mudanças (save changes)
                _context.SaveChanges(); //salvar as mudanças (save changes)
            }

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }


        /// <summary>
        //Buscar o Id no banco utilizando o EF alidar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound, caso contrário retornar OK com a tarefa encontrada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var resultado = _context.Tarefas.Find(id); //Buscar o Id no banco utilizando o EF
            if (resultado == null) //Validar o tipo de retorno.Se não encontrar a tarefa, retornar NotFound
            {
                return NotFound();
            }
            return Ok(resultado);  //caso contrário retornar OK com a tarefa encontrada
        }

        /// <summary>
        /// Buscar todas as tarefas no banco utilizando o EF
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var resultado = _context.Tarefas.ToList(); //Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            if (resultado.Count == 0)
            {
                return NotFound();

            }
            return Ok(resultado);
        }

        /// <summary>
        //   // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
        // Dica: Usar como exemplo o endpoint ObterPorData
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if (titulo == null)
                return BadRequest(new { Erro = "Título não pode estar fazio!" });

            var resultado = _context.Tarefas.Where(a => a.Titulo.Contains(titulo)).ToList(); //Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            if (resultado.Count == 0)
            {
                return NotFound();
                
            }
            return Ok(resultado);
        }


        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }


        /// <summary>
        ///             // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        /// <summary>
        /// TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
        /// TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            //atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Data = tarefa.Data;
            //Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            var alteracao = _context.Update(tarefaBanco);
            var gravacao = _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefaBanco == null)
            {
                return NotFound();
            }
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
