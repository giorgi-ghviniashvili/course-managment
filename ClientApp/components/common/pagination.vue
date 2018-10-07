<template>
  <div class="d-flex  flex-column flex-md-row justify-content-between">
    <div class="d-flex flex-column flex-md-row mb-3 mb-md-0">
      <!-- <div v-if="totalRowsValue > 0" class="d-flex align-items-center justify-content-center">
        <span class="text-gray">Showing {{(currentPageValue - 1) * perPageValue + 1}} to {{endRecord}} of {{totalRowsValue}}</span>
      </div> -->
      <!-- <div v-if="totalRowsValue > 0" class="space-divider"></div>
      <div class="d-flex align-items-center justify-content-center">
        <span> show per page: </span>
        <b-form-select :options="pageOptions" @change="updatePerPage($event)" v-model="perPageValue" class="selectPerPage" />
      </div> -->
    </div>
    <div class="d-flex align-items-center justify-content-center">
      <b-pagination :total-rows="totalRowsValue" :per-page="perPageValue" :prev-text="'Prev'" :next-text="'Next'" @input="$emit('update:currentPage', $event)" v-model="currentPageValue" class="my-0" />
    </div>
  </div>
</template>

<script>
const SET_PERPAGE = 'SET_PERPAGE'

export default {
  name: 'pagination',
  props: ['totalRows', 'currentPage', 'perPage'],
  data () {
    return {
      pageOptions: [ 5, 10, 15, 25, 50, 100 ],
      currentPageValue: this.currentPage,
      perPageValue: this.perPage
    }
  },
  computed: {
    totalRowsValue () {
      return this.totalRows
    },
    endRecord () {
      if (this.perPageValue * this.currentPageValue < this.totalRowsValue) {
        return this.perPageValue * this.currentPageValue
      } else {
        return this.totalRowsValue
      }
    }
  },
  methods: {
    updatePerPage ($event) {
      this.$emit('update:perPage', $event)
      //this.$store.dispatch(SET_PERPAGE, $event)
    }
  }
}
</script>
