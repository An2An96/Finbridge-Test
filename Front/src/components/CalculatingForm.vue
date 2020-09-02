<template>
    <div>
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

            <b-button type="submit" variant="primary">Посчитать</b-button>
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
                results: []
            }
        },
        methods: {
            async onSubmit(evt) {
                evt.preventDefault();

                const numbers = this.form.sequence.split(',').map(x => Number(x));

                this.form.sequence = null;

                const response = await fetch('http://localhost:59816/calculating/sumOfSquares', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        values: numbers
                    })
                });

                if (response.ok) {
                    let body = await response.json();
                    this.results.unshift(body.value);
                }
            }
        }
    }
</script>
