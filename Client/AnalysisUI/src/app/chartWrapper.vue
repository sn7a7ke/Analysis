<template>
    <div class="chart-wrapper border border-info rounded m-2" v-if="isReady">
        <div class="m-2">
            <select class="d-inline font-weight-bold col-2" v-model="verifiedCountry" @change="countryChanged">
                <option v-for="(option, index) in countries" :key="index"
                    v-bind:value="option" >
                    {{ option }}
                </option>
            </select>
            <div class="d-inline col-10">
                <h6 class="d-inline pointer">{{dataType.toUpperCase()}}</h6>
                <h6 v-for="(value, name, index) in summary" :key="index"
                        class="d-inline pointer" :class="isFieldType(index)" 
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
import json from '../assets/timeseries.json'

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
        numeric: function(value) {
            return new Intl.NumberFormat().format(value);
        },
    },
    data(){
        return {
            datacollection: null,
            options: chartOptions,
            rawData: [],
            dailyData: [],
            totalData: [],
            isReady: false,
            verifiedCountry: '',
            _fieldType: '',
            totalKey: 'World',
            summary: {},
            summaryDate: '',
            countries: [],
        }
    },
    mounted() {
        this._fieldType = this.fieldType;
        this.rawData = this.getRawData();
        this.dailyData = this.getConvertData(this.dailyConvertRow);
        this.totalData = this.getConvertData(this.totalConvertRow);
        this.countries = this.getCountries();
        this.verifiedCountry = this.validateCountry();
        this.summary = this.getSummary();
        this.fillData();
        this.isReady = true;
    },
    beforeUpdate() {
    },
    methods: {
        getRawData() {
            return json;
        },
        getConvertData(сonvertRow) {
            var tempData = [];
            Object.keys(this.rawData).forEach( key => {
                let dc = this.rawData[key];
                tempData[key]=сonvertRow(dc);
            });
            this.calculateTotalField(tempData);
            return tempData;
        },
        dailyConvertRow(dc) {
            var tempRow = [];
            for (let i = 1; i < dc.length; i++) {
                tempRow.push(this.diff(dc[i - 1], dc[i]));
            }
            return tempRow
        },
        totalConvertRow(dc) {
            var tempRow = [];
            for (let i = 0; i < dc.length; i++) {
                tempRow.push(dc[i]);
            }
            return tempRow
        },
        calculateTotalField(arr) {
            var tempData = [];
            let keys = Object.keys(arr);
            let dc = arr[keys[0]];
            for (let i = 0; i < dc.length; i++) {
                tempData.push({date: dc[i].date, confirmed: 0, deaths: 0, recovered: 0});
            }
            keys.forEach( key => {
                let dc = arr[key];
                var country = [];
                for (let i = 0; i < dc.length; i++) {
                    tempData[i].confirmed += dc[i].confirmed;
                    tempData[i].deaths += dc[i].deaths;
                    tempData[i].recovered += dc[i].recovered;
                }
            });
            arr[this.totalKey] = tempData;
        },
        diff(di1, di2) {
            return {
                date: di2.date,
                confirmed: di2.confirmed - di1.confirmed,
                deaths: di2.deaths - di1.deaths,
                recovered: di2.recovered - di1.recovered
            };
        },
        fillData() {
            this.datacollection = (this.dataType == dataTypes[0])
                    ? this.selectDate(this.totalData)
                    : this.selectDate(this.dailyData);
        },
        selectDate(arr) {
            return {
                labels: arr[this.verifiedCountry].map(d => d.date),
                datasets: [
                    {
                        backgroundColor: '#888899',
                        data: arr[this.verifiedCountry].map(d => this.getFieldType(d))
                    }
                ]
            }
        },
        getFieldType(d) {
            return d[this._fieldType];
        },
        getSummary() {
            let arr = this.totalData[this.verifiedCountry];
            let le = arr[arr.length-1];
            return {
                confirmed: le.confirmed,
                deaths: le.deaths,
                recovered: le.recovered
            }
        },
        validateCountry() {
            return (this.dailyData[this.country] !== undefined)
                    ? this.country
                    : this.totalKey;
        },
        getCountries() {
            return Object.keys(this.dailyData).sort();
        },
        countryChanged() {
            this.getSummary();
            this.fillData();
        },
        clickFieldType(i) {
            this._fieldType = fieldTypes[i];
            this.fillData();
        },
        isFieldType(i) {
            var expr = fieldTypes[i] == this._fieldType;
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
            this.fillData();
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