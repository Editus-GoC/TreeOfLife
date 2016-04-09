using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TolWcf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly Type errorHandlerType;

        public GlobalErrorBehaviorAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }

        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            //var host = serviceHostBase as ServiceHost;
            //host.AddGenericResolver();
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;

            try
            {
                errorHandler = (IErrorHandler) Activator.CreateInstance(errorHandlerType);
            }
            catch (MissingMethodException e)
            {
                throw new ArgumentException(
                    "The errorHandlerType specified in the ErrorBehaviorAttribute constructor must have a public empty constructor.",
                    e);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException(
                    "The errorHandlerType specified in the ErrorBehaviorAttribute constructor must implement System.ServiceModel.Dispatcher.IErrorHandler.",
                    e);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
    }

    public class GlobalErrorHandler : IErrorHandler
    {
       // private Logger logger = LogManager.GetCurrentClassLogger();
        // Provide a fault. The Message fault parameter can be replaced, or set to
        // null to suppress reporting a fault.

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var newEx = new FaultException(
                string.Format("Exception caught at GlobalErrorHandler{0}Method: {1}{2}Message: {3}",
                    Environment.NewLine, error.TargetSite.Name, Environment.NewLine, error.Message));

            MessageFault msgFault = newEx.CreateMessageFault();
            fault = Message.CreateMessage(version, msgFault, newEx.Action);
        }

        // HandleError. Log an error, then allow the error to be handled as usual.
        // Return true if the error is considered as already handled

        public bool HandleError(Exception error)
        {
            if (error == null)
                return false;
            
            //logger.Error("Log: {0} {1} {2}\n{3}", error.TargetSite.Name, error.GetType().Name, error.Message, error.StackTrace);

            return true;
        }
    }
}