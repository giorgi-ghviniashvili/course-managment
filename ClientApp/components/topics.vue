<template>
  <div>
    <h1>Topics</h1>

    <div v-if="!topics" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <template v-if="topics">
      <table class="table">
        <thead class="bg-dark text-white">
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Unit Id</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr :class="index % 2 == 0 ? 'bg-white' : 'bg-light'" v-for="(topic, index) in topics" :key="index">
            <td>{{ topic.id }}</td>
            <td>{{ topic.description }}</td>
            <td>{{ topic.unitId }}</td>
            <td><a class="btn btn-default" @click="removeTopic(topic.id)">delete</a></td>
          </tr>
        </tbody>
      </table>
      <nav aria-label="...">
        <a class="btn btn-default justify-content-md-end justify-content-sm-center" @click="addRandomTopic()">Add new</a>
        <ul class="pagination justify-content-md-end justify-content-sm-center">
          <li :class="'page-item' + (currentPage == 1 ? ' disabled' : '')">
            <a class="page-link" href="#" tabindex="-1" @click="loadPage(currentPage - 1)">Previous</a>
          </li>
          <li :class="'page-item' + (n == currentPage ? ' active' : '')" v-for="(n, index) in totalPages" :key="index">
            <a class="page-link" href="#" @click="loadPage(n)">{{n}}</a>
          </li>
          <li :class="'page-item' + (currentPage < totalPages ? '' : ' disabled')">
            <a class="page-link" href="#" @click="loadPage(currentPage + 1)">Next</a>
          </li>
        </ul>
      </nav>
    </template>
  </div>
</template>

<script>
 import { mapActions, mapState } from 'vuex'

 export default {
  data() {
    return {
      pageSize: 5,
      currentPage: 1
    }
  },
  computed: {
    ...mapState({
      total: state => state.unitsTotal,
      topics: state => state.topics
    }),
    totalPages: function () {
      return Math.ceil(this.total / this.pageSize)
    }
  },
  methods: {
    ...mapActions(['fetchTopics', 'removeTopic', 'addTopic']),

    async loadPage (page) {
      this.currentPage = page
      var from = (page - 1) * (this.pageSize)
      var to = from + this.pageSize
      this.fetchTopics({
        from,
        to
      })
    },

    async addRandomTopic () {
      this.addTopic({
        description: "wubba labba dub dub",
        unitId: 6  // to Do. Select unit
      })
    }
  },
  async mounted() {
    if (!this.topics) {
      this.loadPage(1)
    }
  }
}
</script>
