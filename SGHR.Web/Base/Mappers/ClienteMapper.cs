using SGHR.Web.Models;

namespace SGHR.Web.Base.Mappers
{
    public static class ClienteMapper
    {
        public static ActualizarClienteModel AModeloActualizar(ClientesModel cliente)
        {
            if (cliente == null) return null;

            return new ActualizarClienteModel
            {
                idCliente = cliente.idCliente,
                nombre = cliente.nombre,
                apellido = cliente.apellido,
                email = cliente.email,
                telefono = cliente.telefono,
                direccion = cliente.direccion
            };
        }

        public static ClientesModel AClientesModel(ActualizarClienteModel model)
        {
            if (model == null) return null;

            return new ClientesModel
            {
                idCliente = model.idCliente,
                nombre = model.nombre,
                apellido = model.apellido,
                email = model.email,
                telefono = model.telefono,
                direccion = model.direccion
            };
        }
    }
}
