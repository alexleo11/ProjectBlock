using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockChain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockChain.Api
{
    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("")]
    public class BlockChainController : Controller
    {
        public static CryptoCurrency blockchain = new CryptoCurrency();

        [HttpPost("transactions/new")]
        public IActionResult new_transaction([FromBody]Transaction transaction)
        {
            var rsp = blockchain.CreateTransaction(transaction);
            return Ok(rsp);
        }


        [HttpGet("transactions/get")]
        public IActionResult get_transactions()//un cambio
        {
            var rsp = new { transactions = blockchain.GetTransactions() };
            return Ok(rsp);
        }

        [HttpGet("chain")]
        public IActionResult full_chain()
        {
            var blocks = blockchain.GetBlocks();
            var rsp = new { chain = blocks, length = blocks.Count };
            return Ok(rsp);
        }
        [HttpGet("mine")]
        public IActionResult mine()
        {
            var block = blockchain.Mine();
            var rsp = new
            {
                message = "New Block Forged",
                block_number = block.Index,
                transactions = block.Transactions.ToArray(),
                nonce = block.Proof,
                previous_hash = block.PrevHash
            };
            return Ok(rsp);
        }

        [HttpPost("nodes/register")]
        public IActionResult register_nodes(string[] nodes)
        {
            blockchain.RegisterNodes(nodes);
            var rsp = new
            {
                message = "New Nodes hav been added",
                total_nodes = nodes.Count() //Total of nodes in blockchain
            };

            return Created("", rsp);
        }
        [HttpGet("nodes/resolve")]
        public IActionResult consensus()
        {
            return Ok(blockchain.Consensus());
        }
        [HttpGet("nodes/get")]
        public IActionResult get_nodes()
        {
            return Ok(new { nodes = blockchain.GetNodes() });
        }
        //mineria//

        [HttpGet("wallet/miner")]
        public IActionResult GetMinersWallet()
        {
            return Ok(blockchain.GetMinersWallet());
        }
        
    }
}