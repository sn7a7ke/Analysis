import { PomApiService } from '../../app/services/pom.api.service'
let pomApiService = new PomApiService();

export default {
    namespaced: true,
    state: {
        mainTable: [],
    },
    getters: {
        mainTable: state => state.mainTable,
    },    
    mutations: {
        setMainTable(state, payLoad) {
            state.mainTable = payLoad;
        },
    },
    actions: {
        getMainTable(store) {            
            return pomApiService.getMainTable()
                .then(mainTable => {
                    store.commit('setMainTable', mainTable);
                });
        },
    },
};
