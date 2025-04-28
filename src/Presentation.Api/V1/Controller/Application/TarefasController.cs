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

[ApiExplorerSettings(GroupName = "Tarefas")]
public class TarefasController(IMediator mediator) : BaseApplicationController
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TarefaDto))]
    public async Task<IActionResult> Post([FromBody] CriarTarefaCommand command)
       => HandlerResponse(HttpStatusCode.Created, await mediator.Send(command));

    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TarefaDto))]
    public async Task<IActionResult> Get(int id)
      => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new ObterTarefaPorIdQuery(id)));

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ServerSideDto<IEnumerable<TarefaListagemDto>>))]
    public async Task<IActionResult> GetAll(int page = 1, int limit = 5)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new ObterTodasTarefasQuery(page, limit)));

    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TarefaDto))]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarTarefaCommand comand)
    {
        comand.Id = id;
        return HandlerResponse(HttpStatusCode.OK, await mediator.Send(comand));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    public async Task<IActionResult> Delete(int id)
        => HandlerResponse(HttpStatusCode.OK, await mediator.Send(new DeletarTarefaCommand(id)));
}
