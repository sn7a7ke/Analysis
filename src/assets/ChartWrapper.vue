<template>
    <div class="chart-wrapper border border-info rounded m-2" v-if="isReady">
        <div class="m-1">
            <select class="d-inline font-weight-bold col-2" v-model="verifiedCountry" @change="countryChanged">
                <option v-for="(option, index) in countries" :key="index"
                    v-bind:value="option" >
                    {{ option }}
                </option>
            </select>
            <h6 class="d-inline col-10">
                <h6 class="d-inline pointer" :class="{ active: dataTypeWr == 0 }" @click="dataTypeWr = 0">
                    Coronavirus Cases: <b>{{summary.confirmed | numeric}}</b>
                </h6>
                <h6 class="d-inline pointer" :class="{ active: dataTypeWr == 1 }" @click="dataTypeWr = 1">
                    Deaths: <b>{{summary.deaths | numeric}}</b>
                </h6>
                <h6 class="d-inline pointer" :class="{ active: dataTypeWr == 2 }" @click="dataTypeWr = 2">
                    Recovered: <b>{{summary.recovered | numeric}}</b>
                </h6>
            </h6>
        </div>
        <div class="m-1">
            <line-chart
                v-if="chartType=='Line'"
                :chart-data="datacollection"
                :options="options">
            </line-chart>
            <bar-chart
                v-if="chartType=='Bar'"
                :chart-data="datacollection"
                :options="options">
            </bar-chart>
        </div>
    </div>
</template>
// <script lang="ts">
//     import DailyInfo from './DailyInfo'
//     export default DailyInfo
// </script>
<script>
import lineChart from './LineChart'
import barChart from './BarChart'
import json from './timeseries.json'

var chartTypes = ['Bar', 'Line']

export default {
    components: {
        lineChart,
        barChart
    },
    props: {
        country: String,
        chartTypeId: Number,
        dataType: Number
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
            fitData: [],
            isReady: false,
            verifiedCountry: '',
            worldKey: 'World',
            summary: {},
            countries: [],
            chartType: '',
            dataTypeWr: null
        }
    },
    mounted() {
        this.dataTypeWr = this.dataType;
        this.chartType = chartTypes[this.chartTypeId];
        this.getRawData();
        this.getFitData();
        this.getCountries();
        this.validateCountry();
        this.getSummary();
        this.fillData();
    },
    beforeUpdate() {
        this.fillData();
    },
    methods: {
        getRawData() {
            this.rawData = json;
        },
        getFitData() {
            var dailyData = [];
            Object.keys(this.rawData).forEach( key => {
                let dc = this.rawData[key];
                var country = [];
                for (let i = 1; i < dc.length; i++) {
                    country.push(this.diff(dc[i - 1], dc[i]));
                }
                dailyData[key]=country;
            });
            this.fitData = dailyData;
            this.calculateWorldData();
        },
        calculateWorldData() {
            var tempData = [];
            let keys = Object.keys(this.fitData);
            let dc = this.fitData[keys[0]];
            for (let i = 0; i < dc.length; i++) {
                tempData.push({date: dc[i].date, confirmed: 0, deaths: 0, recovered: 0});
            }
            keys.forEach( key => {
                let dc = this.fitData[key];
                var country = [];
                for (let i = 0; i < dc.length; i++) {
                    tempData[i].confirmed += dc[i].confirmed;
                    tempData[i].deaths += dc[i].deaths;
                    tempData[i].recovered += dc[i].recovered;
                }
            });
            this.fitData[this.worldKey] = tempData;
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
            this.datacollection = 
            {
                labels: this.fitData[this.verifiedCountry].map(d => d.date),
                datasets: [
                    {
                        backgroundColor: '#888899',
                        data: this.fitData[this.verifiedCountry].map(d => this.getDataType(d))
                    }]
            }
            this.isReady = true;
        },
        getDataType(d) {
            if (this.dataTypeWr == 1) {
                return d.deaths;
            }
            if (this.dataTypeWr == 2) {
                return d.recovered;
            }
            return d.confirmed;
        },
        getSummary() {
            let arr = this.fitData[this.verifiedCountry];
            this.summary.date = arr[arr.length-1].date;
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
            if (this.fitData[this.country] !== undefined) {
                this.verifiedCountry = this.country;
            }
            else {
                this.verifiedCountry = this.worldKey;
            }
        },
        getCountries() {
            this.countries = Object.keys(this.fitData).sort();
        },
        countryChanged() {
            this.getSummary();
        }
    },
    computed: {
    },
    watch: {
        dataType() {
            this.dataTypeWr = this.dataType;
            this.fillData();
        },
        chartTypeId() {
            this.chartType = chartTypes[this.chartTypeId];
        }
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