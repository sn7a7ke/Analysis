<template>
    <div class="chart-wrapper border border-info rounded m-2" v-if="isReady">
        <h3 class="m-1">{{verifiedCountry}}</h3>
        <h5>Coronavirus Cases: {{summary.confirmed | numeric}} Deaths: {{summary.deaths | numeric}} Recovered: {{summary.recovered | numeric}}</h5>
        <div class="m-1">
            <line-chart
                v-if="chartType=='Line'"
                :chart-data="datacollection"
                :options="options"
                :refresh="refresh">
            </line-chart>
            <bar-chart
                v-if="chartType=='Bar'"
                :chart-data="datacollection"
                :options="options"
                :refresh="refresh">
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

export default {
    components: {
        lineChart,
        barChart
    },
    props: {
        country: String,
        chartType: String,
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
                // title: {
                //     display: true,
                //     text: 'Custom Chart Title'
                // },
                scales: {
                    xAxes: [{
                        ticks: {
                            maxRotation: 90,
                            minRotation: 90
                            // min: 'March'
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
            rawData: null,
            fitData: [],
            //worldData: [],
            isReady: false,
            verifiedCountry: '',
            worldKey: 'World',
            summary: {
                date: '',
                confirmed: 0,
                deaths: 0,
                recovered: 0
            },
            refresh: 0
        }
    },
    mounted() {
        this.validateCountry();
        this.getRawData();
        this.calculateData();
        this.calculateWorldData();
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
        calculateData() {
            var dailyData = [];
            Object.keys(this.rawData).forEach( key => {
                // in-case properties are nested objects
                // let value = JSON.stringify(rawData[key]);  
                let dc = this.rawData[key];   // for primitive nested properties
                var country = [];
                for (let i = 1; i < dc.length; i++) {
                    country.push(this.diff(dc[i - 1], dc[i]));
                }
                dailyData[key]=country;
            });
            this.fitData = dailyData;
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
            //this.worldData = tempData;
        },
        diff(di1, di2) {
            return {
                date: di2.date,
                confirmed: di2.confirmed - di1.confirmed,
                deaths: di2.deaths - di1.deaths,
                recovered: di2.recovered - di1.recovered                
            };
        },
        fillData () {
            // var country = this.rawData['Ukraine'];
            // var day = country[95];
            // var death = day.deaths;
            // var test1 = this.fitData['Ukraine'].map(d => d.date);
            // var test2 = this.fitData['Ukraine'].map(d => d.confirmed);
            // debugger;
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
            if (this.dataType == 1) {
                return d.deaths;
            }
            if (this.dataType == 2) {
                return d.recovered;
            }
            return d.confirmed;
        },
        getSummary() {
            let arr = this.fitData[this.verifiedCountry];
            this.summary.date = arr[arr.length-1].date;
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
        }
    },
    computed: {
        dataTypeCnanged() {
            fillData();
        }
    },
}
</script>
<style>
    .chart-wrapper {
        text-align: center;
    }
</style>