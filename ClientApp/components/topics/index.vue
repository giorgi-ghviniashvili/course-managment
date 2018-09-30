<template>
  <div>
    <div v-if="!topics" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <template v-if="topics">
      <div>
        <b-table show-empty
                stacked="md"
                :items="topics"
                :fields="fields"
                :current-page="currentPage"
                :per-page="perPage"
                :sort-by.sync="sortBy"
                :sort-desc.sync="sortDesc"
                :sort-direction="sortDirection"
                class="depth-1"
        >
          <template slot="description" slot-scope="row">
            <b-form-input v-show="row.item.is_edit"  @keyup.native.enter.stop.prevent="saveRow(row.item)" v-model="row.item.edit_description" class="form-control form-control-sm text-regular">
            </b-form-input>
            <span v-show="!row.item.is_edit"  @dblclick="editRow(row.item)">{{ row.item.description }}</span>
          </template>
          <template slot="actions" slot-scope="row">
            <div class="d-flex flex-column flex-md-row align-items-start">
              <button v-show="row.item.is_edit" class='btn plain-btn text-regular' @click.stop="saveRow(row.item)">
                <i class="flaticon stroke checkmark text-success"></i> Save
              </button>
              <button v-show="row.item.is_edit" class='btn plain-btn text-regular' @click.stop="cancelRow(row.item)">
                <i class="fa fa-times text-warning"></i> Cancel
              </button>
              <button v-show="!row.item.is_edit" class='btn plain-btn text-regular' @click.stop="editRow(row.item)">
                <i class="flaticon solid edit-3 text-primary"></i> Edit
              </button>
              <button class='btn btn-default' @click.stop="removeTopic(row.item.id)">
                <i class="flaticon stroke trash-2 text-danger"></i> Delete
              </button>
            </div>
          </template>
          <template slot="HEAD_actions" slot-scope="row">
            <button class='btn btn-sm btn-primary font-weight-bold add-new-row d-none d-sm-block' @click="$refs.modal.show()">
              Add New Topic
            </button>
          </template>
        </b-table>
        <pagination v-if="topics.length > 0" :totalRows="topics.length" :currentPage.sync="currentPage" :perPage.sync="perPage"></pagination>
      </div>
      <div>
        <!-- Modal Component -->
        <b-modal id="modal1"
                 title="Add new topic"
                 ref="modal"
                 @ok="handleOk"
                 @shown="clearDescription">
          <form @submit.stop.prevent="handleSubmit">
            <b-form-group>
              <b-form-select v-model="selectedUnit">
                <option :value="null">Please select an option</option>
                <option v-for="(unit, index) in units"
                        :key="index"
                        :value="unit.id">{{unit.description}}</option>
              </b-form-select>
            </b-form-group>
            <b-form-input type="text"
                          placeholder="Type topic description"
                          v-model="description"></b-form-input>
          </form>
        </b-modal>
      </div>
    </template>
  </div>
</template>

<script>
 import { mapActions, mapState } from 'vuex'
import pagination from '../common/pagination.vue'

 export default {
  data() {
    return {
      fields: [
        { key: 'description', label: 'Topics', sortable: true },
        { key: 'unitName', label: 'unit', sortable: true },
        { key: 'actions', 'class': 'd-lg-flex justify-content-lg-end align-items-center' }
      ],
      description: '',
      currentPage: 1,
      perPage: 5,
      sortDirection: 'asc',
      sortDesc: false,
      selectedUnit: null,
      sortBy: null
    }
  },
  computed: {
    ...mapState({
      topics: state => state.topics,
      units: state => state.units
    })
  },
  methods: {
    ...mapActions(['fetchTopics', 'fetchUnits', 'removeTopic', 'addTopic', 'editTopic']),

    clearDescription() {
      this.description = ''
    },

    handleOk(evt) {
      // Prevent modal from closing
      evt.preventDefault()
      if (!this.description) {
        alert('please type description')
      } else {
        this.handleSubmit()
      }
    },

    handleSubmit() {
      this.addTopic({
        description: this.description,
        unitId: this.selectedUnit
      })
      this.$refs.modal.hide()
    },

    editRow (item) {
      this.$set(item, 'is_edit', true)
      item.edit_description = item.description
    },

    cancelRow (item) {
      this.$set(item, 'is_edit', false)
    },

    async saveRow (item) {
      let data = {
        id: item.id,
        description: item.edit_description,
      }
      await this.editTopic (data)
      this.$set(item, 'is_edit', false)
    }
  },
  async mounted() {
    if (!this.topics) {
      this.fetchTopics()
    }
    if (!this.units) {
      this.fetchUnits()
    }
  },
  components: {
    pagination
  }
}
</script>
