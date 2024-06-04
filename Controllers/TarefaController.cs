using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;
        // Construtor que injeta o contexto do banco de dados
        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Procura a tarefa no banco de dados pelo ID fornecido
            var tarefa = _context.Tarefas.Find(id);

            // Se a tarefa não for encontrada, retorna um status 404 Not Found
            if (tarefa == null)
                return NotFound();

            // Se a tarefa for encontrada, retorna a tarefa com status 200 OK
            return Ok(tarefa);           
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Busca todas as tarefas no banco de dados utilizando o Entity Framework
            List<Tarefa> tarefas = _context.Tarefas.ToList();

            // Retorna a lista de tarefas com status 200 OK
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Busca as tarefas no banco de dados utilizando EF,
            // onde o título da tarefa contém a string recebida por parâmetro
            var tarefa = _context.Tarefas.Where(x => x.Titulo == titulo);
            // Retorna a lista de tarefas com status OK
            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Busca as tarefas no banco de dados utilizando EF
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            // Retorna a lista de tarefas com status OK
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                // Retorna um status 400 Bad Request se a data for inválida
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adiciona a tarefa recebida no contexto do EF
            _context.Add(tarefa);
            
            // Salva as mudanças no banco de dados
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza as informações da tarefa no banco de dados com os dados fornecidos no objeto "tarefa"
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualiza a entrada da tarefa no contexto do Entity Framework e salva as mudanças no banco de 
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
