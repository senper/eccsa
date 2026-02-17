const Form = JSONSchemaForm.default;
const schema = {
    title: "Todo",
    type: "object",
    required: ["title"],
    properties: {
        title: { type: "string", title: "Title", default: "" },
        done: { type: "boolean", title: "Done?", default: false },
        multipleChoice: {
            type: "array",
            maxItems: 1,
            title: "A multiple-choice list",
            items: {
                type: "number",
                enum: [1, 2, 3, 4],
                enumNames: ["foo", "bar", "fuzz", "qux"],
            },
            uniqueItems: true
        },
        select: {
            type: "number",
            maxItems: 1,
            title: "select",
            enum: [1, 2, 3, 4],
            enumNames: ["foo", "bar", "fuzz", "qux"],
        },
    }
};

const formData = {
    done: true,
    multipleChoice: [2, 3],
    select: 4,
    title: "A task"
}

const log = (type) => console.log.bind(console, type);
const onSubmit = ({ formData }, e) => console.log("Data submitted: ", formData);

function CommentBox() {
    return (
        <Form schema={schema} formData={formData}
            onChange={log("changed")}
            onSubmit={onSubmit}//{log("submitted")}
            onError={log("errors")} />
    );
}

ReactDOM.render(<CommentBox />, document.getElementById('reactcontent'));