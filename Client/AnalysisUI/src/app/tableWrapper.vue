<template>
	<div class="table-wrapper">
		<v-card>
			<v-card-title>
				Coronavirus cases
				<v-spacer></v-spacer>
				<v-text-field
					v-model="search"
					append-icon="mdi-magnify"
					label="Search"
					single-line
					hide-details
				></v-text-field>
			</v-card-title>
			<v-data-table
				:headers="headers"
				:items="mainTable"
				:search="search"
				:sort-by="['totalCases']"
				:sort-desc="[true]"
				dense
				item-key="country"
				:items-per-page=20
				:loading="loading"
				loading-text="Loading... Please wait"
				class="elevation-1"
				:footer-props="{
					itemsPerPageOptions: [10, 20, 50, -1],
				}"
			>
				<template v-slot:item.totalCases="{ item }">
					<div>{{item.totalCases | numeric}}</div>
				</template>
				<template v-slot:item.newCases="{ item }">
					<div class="new-value">{{ item.newCases | numericNew}}</div>
				</template>
				<template v-slot:item.totalDeaths="{ item }">
					<div>{{item.totalDeaths | numeric}}</div>
				</template>
				<template v-slot:item.newDeaths="{ item }">
					<div class="new-value">{{ item.newDeaths | numericNew}}</div>
				</template>
				<template v-slot:item.totalRecovered="{ item }">
					<div>{{item.totalRecovered | numeric}}</div>
				</template>
				<template v-slot:item.newRecovered="{ item }">
					<div class="new-value">{{ item.newRecovered | numericNew}}</div>
				</template>
				<template v-slot:item.totalCasesPer1M="{ item }">
					<div>{{ item.totalCasesPer1M | numeric}}</div>
				</template>
				<template v-slot:item.totalDeathsPer1M="{ item }">
					<div>{{ item.totalDeathsPer1M | numeric}}</div>
				</template>


				<!-- <template slot="items" slot-scope="props">
					<tr>
					<td>{{ props.item.country }}</td>
					<td class="text-xs-right">{{ props.item.totalCases | numeric }}</td>
					<td class="new-value">{{ props.item.newCases | numeric }}</td>
					<td class="text-xs-right">{{ props.item.totalDeaths | numeric }}</td>
					<td class="new-value">{{ props.item.newDeaths | numeric }}</td>
					<td class="text-xs-right">{{ props.item.totalRecovered | numeric }}</td>
					<td class="new-value">{{ props.item.newRecovered | numeric }}</td>
					<td class="text-xs-right">{{ props.item.totalCasesPer1M | numeric }}</td>
					<td class="text-xs-right">{{ props.item.totalDeathsPer1M | numeric }}</td>
					</tr>
				</template> -->
			</v-data-table>
		</v-card>
	</div>
</template>

<script>
import { mapGetters } from 'vuex';
import { mapActions } from 'vuex';

export default {
	filters: {
        numeric(value) {
			if (value == 0 ) return '';
            return new Intl.NumberFormat().format(value);
		},
		numericNew(value) {
			if (value == 0 ) return '';
            return new Intl.NumberFormat('en-GB', {signDisplay: 'exceptZero'}).format(value);
        },
	},
	data () {
		return {
			loading: true,
			search: '',
			headers: [
				{ text: 'Country', align: 'start', value: 'country', width: 150 },
				{ text: 'Total cases', align: 'end', value: 'totalCases' },
				{ text: 'New cases', align: 'end', value: 'newCases' },
				{ text: 'Total deaths', align: 'end', value: 'totalDeaths' },
				{ text: 'New deaths', align: 'end', value: 'newDeaths' },
				{ text: 'Total recovered', align: 'end', value: 'totalRecovered' },
				{ text: 'New recovered', align: 'end', value: 'newRecovered' },
				{ text: 'Total cases per 1M', align: 'end', value: 'totalCasesPer1M' },
				{ text: 'Total deaths per 1M', align: 'end', value: 'totalDeathsPer1M' },
			],
		}
	},
	computed: {
        ...mapGetters('covidTable', {
                mainTable: 'mainTable',
		}),
	},
	mounted() {
        this.getMainTable().then(result => {
				this.loading = false;
			});
    },
	methods: {
        ...mapActions('covidTable', {
            getMainTable: 'getMainTable',
		}),
	}
}
</script>
<style>
.v-data-table th {
	font-size: 14px;
	padding: 0 8px;
}
.v-data-table td {
	font-size: 15px;
	font-weight: bold;
	padding: 0 4px;
}
.v-data-table td div{
	padding: 4px;
}
.new-value {
	background-color: #f2f7fd;
	height: 100%;
}
</style>