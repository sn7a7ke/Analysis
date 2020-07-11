const axios = require('axios');

export class PomApiService {
    constructor() {
        axios.defaults.baseURL = 'https://localhost:44330/';
    }

    getRaw() {
        let url = "PomCovidRaw/Json";
        return this.getHttpRequest(url);
    }

    getAll(dataType) {
        let url = "PomCovid" + dataType + "/All";
        return this.getHttpRequest(url);
    }

    getAll(dataType, country) {
        let url = "PomCovid" + dataType + "/All/";
        return this.getHttpRequest(url + country);
    }

    getSummary(dataType) {
        let url = "PomCovid" + dataType + "/Summary";
        return this.getHttpRequest(url);
    }

    getSummary(dataType, country) {
        let url = "PomCovid" + dataType + "/Summary/";
        return this.getHttpRequest(url + country);
    }

    getAllCountries(dataType) {
        let url = "PomCovid" + dataType + "/AllCountries";
        return this.getHttpRequest(url);
    }

    getMainTable() {
        let url = "PomCovidTable/All";
        return this.getHttpRequest(url);
    }

    getHttpRequest(url) {
        return axios
            .get(url)
            .then(response => {
                return response.data;
            })
            .catch(error => {
                console.log(error);
            });    
    }
}
