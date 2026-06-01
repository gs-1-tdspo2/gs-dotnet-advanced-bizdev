using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amanaje_API.Models.Externals
{
    [Table("TB_AMANAJE_CLI")]
    public class ClienteExternal
    {
        [Key]
        [Column("ID_CLIENTE")]
        public int Id { get; set; }
    }

    [Table("TB_AMANAJE_USU")]
    public class UsuarioExternal
    {
        [Key]
        [Column("ID_USUARIO")]
        public int Id { get; set; }
    }
}