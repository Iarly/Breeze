﻿(function () {
    'use strict';
    angular.module('app').factory(
        'entityManagerProvider', ['config', 'model', emProvider]);

    function emProvider(config, model) {
        var serviceName = config.serviceName;
        var metadataStore = getMetadataStore();
        
        return {
            newManager: newManager
        };
        
        function getMetadataStore() {
            var store = new breeze.MetadataStore();

            // Import metadata that were downloaded as a script file
            // Because of Breeze bug, must stringify it first.
            store.importMetadata(JSON.stringify(zza.metadata));

            // Associate these metadata data with the service
            store.addDataService(
                new breeze.DataService({ serviceName: serviceName }));           

            model.configureMetadataStore(store);

            return store;
        }
        
        function newManager() {
            var mgr = new breeze.EntityManager({
                serviceName: serviceName,
                metadataStore: metadataStore
            });
            mgr.enableSaveQueuing(true);
            return mgr;
        }
   }

})();