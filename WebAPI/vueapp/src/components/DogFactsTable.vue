<template>
    <div class="container">
        <h1>Dog Facts</h1>

        <!-- Error Message -->
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

        <!-- Loading Indicator -->
        <p v-if="loading">Loading dog facts...</p>

        <!-- Filters -->
        <div class="filters">
            <input v-model.trim="filterId"
                   type="text"
                   placeholder="Filter by ID" />
            <input v-model.trim="filterBody"
                   type="text"
                   placeholder="Filter by Fact" />
        </div>

        <!-- Table -->
        <table v-if="!loading && filteredDogFacts.length">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Fact</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="fact in filteredDogFacts" :key="fact.id">
                    <td>{{ fact.id }}</td>
                    <td>{{ fact.body }}</td>
                </tr>
            </tbody>
        </table>

        <!-- No Data Message -->
        <p v-if="!loading && !filteredDogFacts.length && !errorMessage">
            No dog facts found.
        </p>
    </div>
</template>

<script>
    export default {
        name: "DogFactsTable",
        data() {
            return {
                dogFacts: [],      // Stores fetched dog facts
                filterId: "",      // User input filter for ID
                filterBody: "",    // User input filter for Fact
                loading: false,    // Indicates loading state
                errorMessage: ""   // Stores API error messages
            };
        },
        computed: {
            filteredDogFacts() {
                // Convert filters to lowercase once for better performance
                const idFilter = this.filterId.toLowerCase();
                const bodyFilter = this.filterBody.toLowerCase();

                return this.dogFacts.filter(fact => {
                    const matchId = fact.id.toString().toLowerCase().includes(idFilter);
                    const matchBody = fact.body.toLowerCase().includes(bodyFilter);
                    return matchId && matchBody;
                });
            }
        },
        async mounted() {
            this.loading = true;
            try {
                const response = await fetch("https://localhost:7194/api/dogfacts");
                if (!response.ok) {
                    throw new Error(`API request failed with status ${response.status}`);
                }
                this.dogFacts = await response.json();
            } catch (error) {
                console.error("Error fetching DogFacts:", error);
                this.errorMessage = "Failed to load Dog Facts. Please try again later.";
            } finally {
                this.loading = false;
            }
        }
    };
</script>

<style scoped>
    .container {
        max-width: 800px;
        margin: 20px auto;
        padding: 10px;
    }

    .filters {
        margin-bottom: 10px;
    }

        .filters input {
            margin-right: 10px;
            padding: 5px;
        }

    table {
        width: 100%;
        border-collapse: collapse;
    }

    th, td {
        padding: 8px;
        border: 1px solid #ccc;
    }

    th {
        background-color: #f4f4f4;
        text-align: left;
    }

    .error {
        color: red;
        font-weight: bold;
        margin-bottom: 10px;
    }
</style>
