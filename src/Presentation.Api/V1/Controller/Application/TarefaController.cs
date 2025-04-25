using Application.Commands.AtualizarTarefa;
using Application.Commands.CriarTarefa;
using Application.Commands.DeletarTarefa;
using Application.DTOs;
using Application.Queries.ObterTarefaPorId;
using Application.Queries.ObterTodasTarefas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Api.Controllers._Shared;
using System.Net;

namespace Presentation.Api.V1.Controller.Application;

[ApiExplorerSettings(GroupName = "Tarefa")]
public class TarefaController(IMediator mediator) : BaseApplicationController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<TarefaDto>))]
    public async Task<IActionResult> Post([FromBody] CriarTarefaCommand command)
       => HandlerResponse(HttpStatusCode.Created, await mediator.Send(command));

    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<TarefaDto>))]
    public async Task<IActionResult> Get(int id)
      => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new ObterTarefaPorIdQuery(id)));

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<TarefaDto>>))]
    public async Task<IActionResult> GetAll()
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new ObterTodasTarefasQuery()));

    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<TarefaDto>))]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaCommand comand)
    {
        comand.Id = id;
        return HandlerResponse(HttpStatusCode.OK, await mediator.Send(comand));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<string>))]
    public async Task<IActionResult> Delete(int id)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new DeletarTarefaCommand(id)));
}
