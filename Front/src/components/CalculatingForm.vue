<template>
    <div>
        <b-alert v-if="error" show variant="danger">{{error}}</b-alert>

        <b-form @submit="onSubmit">
            <b-form-group
                    id="input-group-1"
                    label="Последовательность:"
                    label-for="input-1"
            >
                <b-form-input
                        id="input-1"
                        v-model="form.sequence"
                        required
                        placeholder="Введите последовательность чисел"
                ></b-form-input>
            </b-form-group>

            <b-button v-if="!loading" type="submit" variant="primary">Посчитать</b-button>
            <b-button v-else disabled type="submit" variant="primary">Вычисление...</b-button>
        </b-form>
        <b-card class="mt-3" header="Результаты">
            <pre v-for="item in results" class="m-0">{{ item }}</pre>
        </b-card>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                form: {
                    sequence: ''
                },
                loading: false,
                error: null,
                results: []
            }
        },
        methods: {
            async onSubmit(evt) {
                evt.preventDefault();

                if (!/(-?\d\s?,\s?)+-?\d/.test(this.form.sequence)) {
                    alert('Неверная последовательность: укажите набор чисел через запятую');
                    return;
                }

                const numbers = this.form.sequence.split(',').map(x => Number(x));

                this.error = null;
                this.loading = true;
                const response = await fetch('http://localhost:59816/calculating/sumOfSquares', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        values: numbers
                    })
                });

                let body = await response.json();
                if (response.ok) {
                    this.results.unshift(body.value);
                    this.form.sequence = '';
                } else {
                    this.error = body.message;
                }

                this.loading = false;
            }
        }
    }
</script>
