using System;

namespace ZQNB.Web.Models
{
    public interface ICrudViewModel : IGuidPk
    {
    }

    public interface IGuidPk
    {
        Guid Id { get; set; }
    }
}
