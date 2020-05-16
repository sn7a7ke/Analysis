import Vue from 'vue'
import Vuex from 'vuex'

import {PomApiService} from '../app/services/pom.api.service'
let pomApiService = new PomApiService();

Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {
        countries: [],
        summary: {},
        countryData: {},
    },
    getters: {
        count: state => state.count,
        countries: state => state.countries,
        summaryByCountry: state => country => state.summary[country],
        countryDataByCountry: state => country => state.countryData[country],
    },    
    mutations: {
        setCountries(state, payLoad) {
            state.countries = payLoad;
        },
        setSummary(state, payLoad) {
            Vue.set(state.summary, payLoad.country, payLoad.summary);
        },
        setCountryData(state, payLoad) {
            Vue.set(state.countryData, payLoad.country, payLoad.countryData);
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
            return pomApiService.getSummary(payLoad.dataType, country)
                .then(summary => {
                    store.commit('setSummary', {summary, country});
                });
        },
        getCountryData(store, payLoad) {
            let country = payLoad.country;
            return pomApiService.getAll(payLoad.dataType, country)
                .then(countryData => {
                    store.commit('setCountryData', {countryData, country});
                });
        },
    },
    strict: process.env.NODE_ENV !== 'production',
});