<template>
    <div class="chart-wrapper border border-info rounded m-2 p-2" v-if="isReady">
        <div class="d-flex m-1 align-items-center">
            <v-select class="country-dropdown p-1" 
                :options="countries"
                :clearable="false"
                v-model="verifiedCountry" 
                @input="countryChanged">
            </v-select>
            <div class="d-flex col p-1 align-items-center">
                <div class="summary pointer p-1"  :class="dataTypeClass"
                        @mouseover="dataTypeMouseover"
                        @click="dataTypeClick()">
                    {{_dataType.toUpperCase()}}
                </div>
                <div class="summary pointer p-1" :class="isFieldType(index)"
                        v-for="(value, name, index) in summary" :key="index"
                        @click="fieldTypeClick(index)">
                    {{name | capitalizeFirstLetter}}: <b>{{value | numeric}}</b>
                </div>
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
import vSelect from 'vue-select'
import {chartTypes, fieldTypes, dataTypes} from './common/constants.ts'
import {chartOptions} from './common/chartOptions.ts'
import LineChart from './lineChart'
import BarChart from './barChart'
import {PomApiService} from './services/pom.api.service.js'

export default {
    components: {
        vSelect,
        LineChart,
        BarChart
    },
    props: {
        country: {
            type: String,
            default: this.defaultCountry,
        },
        chartType: {
            type: String,
            default: chartTypes[0],
            validator(value) {
                return chartTypes.includes(value);
            }
        },
        fieldType: {
            type: String,
            default: fieldTypes[0],
            validator(value) {
                return fieldTypes.includes(value);
            }
        },
        dataType: {
            type: String,
            default: dataTypes[0],
            validator(value) {
                return dataTypes.includes(value);
            }
        },
    },
    filters: {
        numeric(value) {
            return new Intl.NumberFormat().format(value);
        },
        capitalizeFirstLetter(value) {
            return value[0].toUpperCase() + value.substring(1).toLowerCase();
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
            defaultCountry: 'World',
            summary: {},
            countries: [],
            pomApiService: null,
            dataTypeClassToggler: true
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
                    : this.defaultCountry;
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
        fieldTypeClick(i) {
            this._fieldType = fieldTypes[i];
            this.fillData();
        },
        dataTypeClick() {
            this._dataType = this.getNextElement(dataTypes, this._dataType);
            this.dataTypeClassToggler = true;
            setTimeout(() => this.dataTypeClassToggler = false, 1000);
            this.refresh();
        },
        getNextElement(arr, el) {
            let idx = arr.findIndex(x => x == el);
            let nextIdx = (idx + 1) % arr.length;
            return arr[nextIdx];
        },
        isFieldType(i) {
            let expr = fieldTypes[i] == this._fieldType;
            return {'active': expr, 'animate__animated': expr, 'animate__heartBeat': expr};
        },
        dataTypeMouseover() {
            this.dataTypeClassToggler = false;
        },
    },
    computed: {
        dataTypeClass() {
            return {'animate__animated': this.dataTypeClassToggler, 'animate__heartBeat': this.dataTypeClassToggler};
        },
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
@import '../../node_modules/vue-select/dist/vue-select.css';
.chart-wrapper {
    text-align: left;
}
.active {
    background-color: #ddddee;
    border-radius: 5px;
}
.pointer:hover {
    cursor: pointer;
}
.summary {
    font-size: large;
    text-align: center;
}
.country-dropdown {
    min-width: 250px;    
}
</style>