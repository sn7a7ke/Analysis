import Vue from 'vue'

import { getKey } from '../../app/common/functions'
import { PomApiService } from '../../app/services/pom.api.service'
let pomApiService = new PomApiService();

export default {
    namespaced: true,
    state: {
        countries: [],
        summary: {},
        countryData: {},
    },
    getters: {
        count: state => state.count,
        countries: state => state.countries,
        summaryByKey: state => key => state.summary[key],
        countryDataByKey: state => key => state.countryData[key],
    },    
    mutations: {
        setCountries(state, payLoad) {
            state.countries = payLoad;
        },
        setSummary(state, payLoad) {
            Vue.set(state.summary, payLoad.key, payLoad.summary);
        },
        setCountryData(state, payLoad) {
            Vue.set(state.countryData, payLoad.key, payLoad.countryData);
        },
    },
    actions: {
        getAllCountries(store, dataType) {
            return pomApiService.getAllCountries(dataType)
                    .then(result => {
                        store.commit('setCountries', result);
                    });
        },
        getSummary(store, payLoad) {
            let country = payLoad.country;
            let dataType = payLoad.dataType;
            let key = getKey(country, dataType);
            if (store.getters.summaryByKey(key) !== undefined)
                return Promise.resolve("Cached");
            return pomApiService.getSummary(dataType, country)
                .then(summary => {
                    store.commit('setSummary', {key, summary});
                });
        },
        getCountryData(store, payLoad) {
            let country = payLoad.country;
            let dataType = payLoad.dataType;
            let key = getKey(country, dataType);
            if (store.getters.countryDataByKey(key) !== undefined)
                return Promise.resolve("Cached");
            return pomApiService.getAll(dataType, country)
                .then(countryData => {
                    store.commit('setCountryData', {key, countryData});
                });
        },
    },
};
