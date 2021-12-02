using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiProjet.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class ProjetController : ControllerBase
    {
        private readonly WebApiProjet.Models.SiteProjetWebV2Context _context;
        public ProjetController (WebApiProjet.Models.SiteProjetWebV2Context context)
        {
            _context = context;
        }
        // GET: api/Idcomment
        //lister les id 
        [HttpGet("private-scoped")]
        [Route("Idcomment")]
        [Authorize("read:messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<int>>> GetAllId()
        {
            return await _context.Comments.Select(id=>id.Id).ToListAsync();
        }
        // GET: api/comment
        //lister les commentaires
        [HttpGet("private-scoped")]
        [Route("comment")]
        [Authorize("edit:messages")]      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.Comment>>> GetAllComment()
        {
            return await _context.Comments.ToListAsync();
        }
        // GET: api/comment/5
        //voir les commentaires d'une photo
        [HttpGet("{id}")]
        [HttpGet("private-scoped")]
        [Route("comment/{id}")]
        [Authorize("read:messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Models.Comment>>> GetCommentById(string id)
        {
            var todo = await _context.Comments.Where(i =>i.UrlPhoto == id).ToListAsync();

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }
        // GET: api/commentById/5
        //voir un  commentaires via son id
        [HttpGet("private-scoped")]
        [HttpGet("{id}")]
        [Authorize("edit:messages")]
        [Route("commentById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Models.Comment>> GetaCommentById(int id)
        {
            var todo = await _context.Comments.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }
        // POST api/<ValuesController>
        //poster un commentaire
        [HttpPost("private-scoped")]
        [Authorize("read:messages")]
        [Route("comment/post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Models.Comment>> PostComment(Models.Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentById", new { id = comment.UrlPhoto }, comment);
        }
        // modifier un commentaire
        // Put api/comment/update/{id}
        [Route("comment/update/{id}")]
        [HttpPut("private-scoped")]
        [HttpPut("{id}")]
        [Authorize("edit:messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateComment(int id, Models.Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE api/comment/delete/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("comment/delete/{id}")]
        [HttpDelete("{id}")]
        [HttpDelete("private-scoped")]
        [Authorize("delete:messages")]
        public async Task<ActionResult<Models.Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
