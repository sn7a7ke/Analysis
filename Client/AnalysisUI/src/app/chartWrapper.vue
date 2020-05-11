<template>
    <div class="chart-wrapper border border-info rounded m-2" v-if="isReady">
        <div class="m-2">
            <select class="d-inline font-weight-bold col-2" 
                    v-model="verifiedCountry" @change="countryChanged">
                <option v-for="(option, index) in countries" :key="index"
                    v-bind:value="option" >
                    {{ option }}
                </option>
            </select>
            <div class="d-inline col-10">
                <h6 class="d-inline pointer" @click="clickDataType()">
                    {{_dataType.toUpperCase()}}
                </h6>
                <h6 class="d-inline pointer" :class="isFieldType(index)"
                        v-for="(value, name, index) in summary" :key="index"
                        @click="clickFieldType(index)">
                    {{name}}: <b>{{value | numeric}}</b>
                </h6>
            </div>
        </div>
        <div class="m-1">
            <line-chart
                v-if="chartType=='line'"
                :chart-data="datacollection"
                :options="options">
            </line-chart>
            <bar-chart
                v-if="chartType=='bar'"
                :chart-data="datacollection"
                :options="options">
            </bar-chart>
        </div>
    </div>
</template>

<script>
import {chartTypes, fieldTypes, dataTypes} from './common/constants.ts'
import {chartOptions} from './common/chartOptions.ts'
import LineChart from './lineChart'
import BarChart from './barChart'
import {PomApiService} from './services/pom.api.service.js'

export default {
    components: {
        LineChart,
        BarChart
    },
    props: {
        country: String,
        chartType: String,
        fieldType: String,
        dataType: String,
    },
    filters: {
        numeric(value) {
            return new Intl.NumberFormat().format(value);
        },
    },
    data(){
        return {
            datacollection: null,
            options: chartOptions,
            isReady: false,
            verifiedCountry: '',
            countryData: [],
            _fieldType: '',
            _dataType: '',
            totalKey: 'World',
            summary: {},
            countries: [],
            pomApiService: null,
        }
    },
    mounted() {
        this._fieldType = this.fieldType;
        this._dataType = this.dataType;
        this.pomApiService = new PomApiService();
        this.initializeComponent();
    },
    beforeUpdate() {
    },
    methods: {
        initializeComponent() {
            return this.pomApiService.getAllCountries(this._dataType)
                .then(result => {
                    this.countries = result;
                    this.verifyCountry();
                    return this.refresh();
                })
                .then(result => {
                    this.isReady = true;
                });
        },
        verifyCountry() {
            this.verifiedCountry = (this.countries.includes(this.country))
                    ? this.country
                    : this.totalKey;
        },
        refresh() {
            return Promise.all([this.getSummary(), this.getData()])
                .then(result => {
                    this.fillData();
                });
        },
        getSummary() {
            return this.pomApiService.getSummary(this._dataType, this.verifiedCountry)
                .then(result => {
                    this.summaryMap(result);
                });
        },
        summaryMap(result) {
            this.summary.confirmed = result.confirmed;
            this.summary.deaths = result.deaths;
            this.summary.recovered = result.recovered;
        },
        getData() {
            return this.pomApiService.getAll(this._dataType, this.verifiedCountry)
                .then(result => {
                        this.countryData = result;
                });
        },
        fillData() {
            this.datacollection = {
                labels: this.countryData.map(d => d.date),
                datasets: [{
                    backgroundColor: '#888899',
                    data: this.countryData.map(d => d[this._fieldType])
                }]
            }
        },
        countryChanged() {
            this.refresh();
        },
        clickFieldType(i) {
            this._fieldType = fieldTypes[i];
            this.fillData();
        },
        clickDataType() {
            this._dataType = this.getNextElement(dataTypes, this._dataType);
            this.refresh();
        },
        getNextElement(arr, el) {
            let idx = arr.findIndex(x => x == el);
            let nextIdx = (idx + 1) % arr.length;
            return arr[nextIdx];
        },
        isFieldType(i) {
            let expr = fieldTypes[i] == this._fieldType;
            return {'active': expr};
        },
    },
    computed: {
    },
    watch: {
        fieldType() {
            this._fieldType = this.fieldType;
            this.fillData();
        },
        dataType() {
            this._dataType = this.dataType
            this.refresh();
        },
    }
}
</script>

<style scoped>
.chart-wrapper {
    text-align: left;
}
.active {
    background-color: #ddddee;
}
.pointer:hover {
    cursor: pointer;
}
</style>