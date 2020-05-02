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
import {chartTypes, fieldTypes, dataTypes} from './Common/Constants.js'
import lineChart from './LineChart'
import barChart from './BarChart'
import json from './timeseries.json'

export default {
    components: {
        lineChart,
        barChart
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
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    xAxes: [{
                        ticks: {
                            maxRotation: 90,
                            minRotation: 90
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            suggestedMin: 0,
                        }
                    }]
                },
                legend: {
                    display: false,
                }
            },
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
        this.getRawData();
        this.getDailyData();
        this.getTotalData();
        this.getCountries();
        this.validateCountry();
        this.getSummary();
        this.fillData();
        this.isReady = true;
    },
    beforeUpdate() {
    },
    methods: {
        getRawData() {
            this.rawData = json;
        },
        getDailyData() {
            var tempData = [];
            Object.keys(this.rawData).forEach( key => {
                let dc = this.rawData[key];
                var country = [];
                for (let i = 1; i < dc.length; i++) {
                    country.push(this.diff(dc[i - 1], dc[i]));
                }
                tempData[key]=country;
            });
            this.dailyData = tempData;
            this.calculateTotalField(this.dailyData);
        },
        getTotalData() {
            var tempData = [];
            Object.keys(this.rawData).forEach( key => {
                let dc = this.rawData[key];
                var country = [];
                for (let i = 0; i < dc.length; i++) {
                    country.push(dc[i]);
                }
                tempData[key]=country;
            });
            this.totalData = tempData;
            this.calculateTotalField(this.totalData);
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
            let arr = this.dailyData[this.verifiedCountry];
            this.summaryDate = arr[arr.length-1].date;
            this.summary.confirmed = 0;
            this.summary.deaths = 0;
            this.summary.recovered = 0;
            for (let i = 0; i < arr.length; i++) {
                this.summary.confirmed += arr[i].confirmed;
                this.summary.deaths += arr[i].deaths;
                this.summary.recovered += arr[i].recovered;
            }
        },
        validateCountry() {
            if (this.dailyData[this.country] !== undefined) {
                this.verifiedCountry = this.country;
            }
            else {
                this.verifiedCountry = this.totalKey;
            }
        },
        getCountries() {
            this.countries = Object.keys(this.dailyData).sort();
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

<style>
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