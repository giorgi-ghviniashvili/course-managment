<template>
  <div>
    <div v-if="!units" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <template v-if="units">
      <div>
        <b-table show-empty
                 stacked="md"
                 ref="table"
                 :items="unitsProvider"
                 :fields="fields"
                 :current-page="currentPage"
                 :per-page="perPage"
                 :sort-by.sync="sortBy"
                 :sort-desc.sync="sortDesc"
                 :sort-direction="sortDirection"
                 class="depth-1">
          <template slot="description" slot-scope="row">
            <b-form-input v-show="row.item.is_edit" @keyup.native.enter.stop.prevent="saveRow(row.item)" v-model="row.item.edit_description" class="form-control form-control-sm text-regular">
            </b-form-input>
            <span v-show="!row.item.is_edit" @dblclick="editRow(row.item)">{{ row.item.description }}</span>
          </template>
          <template slot="sequence" slot-scope="row">
            <b-form-input v-show="row.item.is_edit" @keyup.native.enter.stop.prevent="saveRow(row.item)" v-model="row.item.edit_sequence" class="form-control form-control-sm text-regular">
            </b-form-input>
            <span v-show="!row.item.is_edit" @dblclick="editRow(row.item)">{{ row.item.sequence }}</span>
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
              <button class='btn btn-default' @click.stop="removeUnit(row.item.id)">
                <i class="flaticon stroke trash-2 text-danger"></i> Delete
              </button>
            </div>
          </template>
          <template slot="HEAD_actions" slot-scope="row">
            <button class='btn btn-sm btn-primary font-weight-bold add-new-row d-none d-sm-block' @click="$refs.modal.show()">
              Add New Unit
            </button>
          </template>
        </b-table>
        <pagination v-if="units.length > 0" :totalRows="totalUnits" :currentPage.sync="currentPage" :perPage.sync="perPage"></pagination>
      </div>
      <div>
        <!-- Modal Component -->
        <b-modal id="modalUnit"
                 title="Add new unit"
                 ref="modal"
                 @ok="handleOk"
                 @shown="clearForm">
          <form @submit.stop.prevent="handleSubmit">
            <b-form-group>
              <b-form-select v-model="selectedCourse"
                             :state="!$v.selectedCourse.$invalid"
                             aria-describedby="courseFeedback">
                <option :value="null">Please select an option</option>
                <option v-for="(course, index) in coursesAll"
                        :key="index"
                        :value="course.id">
                  {{course.description}}
                </option>
              </b-form-select>
              <b-form-invalid-feedback id="courseFeedback">
                Choose course
              </b-form-invalid-feedback>
            </b-form-group>
            <b-form-group>
              <b-form-input type="text"
                            placeholder="Type unit description"
                            v-model="description"
                            :state="!$v.description.$invalid"
                            aria-describedby="descrFeedback"></b-form-input>
              <b-form-invalid-feedback id="descrFeedback">
                Fill description
              </b-form-invalid-feedback>
            </b-form-group>
            <b-form-group>
              <b-form-input type="number"
                            placeholder="Type sequence"
                            v-model="sequence"></b-form-input>
            </b-form-group>
          </form>
        </b-modal>
      </div>
    </template>
  </div>
</template>

<script>
 import { mapActions, mapState } from 'vuex'
 import { required } from 'vuelidate/lib/validators'
 import pagination from '../common/pagination.vue'
 export default {
  data() {
    return {
      fields: [
        { key: 'description', label: 'Units', sortable: true },
        { key: 'sequence', label: 'sequence', sortable: true },
        { key: 'courseName', label: 'course', sortable: true },
        { key: 'actions', 'class': 'd-lg-flex justify-content-lg-end align-items-center' }
      ],
      description: '',
      currentPage: 1,
      perPage: 5,
      sortDirection: 'asc',
      sortDesc: false,
      selectedCourse: null,
      sortBy: null,
      sequence: 0
    }
  },
  validations: {
    selectedCourse: {
      required
    },
    description: {
      required
    }
  },
  computed: {
    ...mapState({
      totalUnits: state => state.unitsTotal,
      units: state => state.units,
      coursesAll: state => state.coursesAll
    })
  },
  methods: {
    ...mapActions(['fetchUnits', 'getAllCourses', 'removeUnit', 'addUnit', 'editUnit']),
    unitsProvider () {
      return this.units ? this.units : []
    },
    async loadPage (page) {
      var from = (page - 1) * (this.perPage)
      var to = from + this.perPage
      this.fetchUnits({
        from,
        to
      })
    },

    clearForm() {
      this.description = ''
      this.sequence = 0
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
      this.addUnit({
        description: this.description,
        sequence: this.sequence,
        courseId: this.selectedCourse
      })
      this.$refs.modal.hide()
    },
    editRow (item) {
      this.$set(item, 'is_edit', true)
      item.edit_description = item.description
      item.edit_sequence = item.sequence
    },
    cancelRow (item) {
      this.$set(item, 'is_edit', false)
    },
    async saveRow (item) {
      let data = {
        id: item.id,
        description: item.edit_description,
        sequence: item.sequence
      }
      await this.editUnit (data)
      this.$set(item, 'is_edit', false)
    }
  },
  watch: {
    async currentPage () {
      await this.loadPage(this.currentPage)
    },
    units () {
      if (this.$refs && this.$refs.table) {
        this.$refs.table.refresh();
      }
    }
  },
  async mounted() {
    if (!this.units) {
      this.loadPage(1)
    }
    if (!this.coursesAll) {
      this.getAllCourses()
    }
  },
  components: {
    pagination
  }
}
</script>
