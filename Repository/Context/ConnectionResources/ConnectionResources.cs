using System.ComponentModel;

namespace Repository.Context.ConnectionResources
{
    public enum DeploymentResources
    {
        /// <summary>
        /// This represents standard local connection strings.
        /// It's typically used when the application is deployed in a local environment 
        /// where the database server is either installed on the same machine 
        /// or accessible via the local network.
        /// </summary>
        [Description("DApplication_Connection")]
        ConnectionStrings,

        /// <summary>
        /// This represents Docker connection strings.
        /// It's used when the application is deployed in a Docker container.
        /// The connection string will point to a SQL Server instance running in a separate Docker container.
        /// It's important to note that Docker containers can communicate with each other 
        /// if they are in the same Docker network.
        /// </summary>
        [Description("DApplication_Connection_Docker")]
        DockerConnectionStrings

        /*
        ...
        Additional deployment resources can be added here as the application evolves.
        For example, you might add AzureConnectionStrings for deployments in Azure,
        or KubernetesConnectionStrings for deployments in a Kubernetes cluster.
        */
    }

}
