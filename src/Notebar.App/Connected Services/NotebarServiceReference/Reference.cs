﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Notebar.App.NotebarServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NotebarServiceReference.INotebarService")]
    public interface INotebarService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotebarService/AddIndicator", ReplyAction="http://tempuri.org/INotebarService/AddIndicatorResponse")]
        string AddIndicator(uint port);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotebarService/AddIndicator", ReplyAction="http://tempuri.org/INotebarService/AddIndicatorResponse")]
        System.Threading.Tasks.Task<string> AddIndicatorAsync(uint port);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INotebarServiceChannel : Notebar.App.NotebarServiceReference.INotebarService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NotebarServiceClient : System.ServiceModel.ClientBase<Notebar.App.NotebarServiceReference.INotebarService>, Notebar.App.NotebarServiceReference.INotebarService {
        
        public NotebarServiceClient() {
        }
        
        public NotebarServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NotebarServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotebarServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotebarServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string AddIndicator(uint port) {
            return base.Channel.AddIndicator(port);
        }
        
        public System.Threading.Tasks.Task<string> AddIndicatorAsync(uint port) {
            return base.Channel.AddIndicatorAsync(port);
        }
    }
}
