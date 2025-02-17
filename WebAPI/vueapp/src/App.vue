<template>
    <div id="app">
        <DogFactsTable />
    </div>
</template>

<script>
import DogFactsTable from "./components/DogFactsTable.vue";

export default {
  name: "App",
  components: {
    DogFactsTable
  }
};

</script>