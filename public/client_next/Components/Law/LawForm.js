import { Form, Button, InputGroup } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { lawTypes } from "../../pages/api/laws";

export default function LawForm({
  selectedLawType,
  setSelectedLawType,
  loadLaws,
  getLocalByVal
}) {
  function searchSubmit(event) {
    event.preventDefault();
    loadLaws(selectedLawType);
  }
  return (
    <Form onSubmit={searchSubmit}>
      <InputGroup style={{ marginTop: "10px" }}>
        <Form.Select
          md={1}
          style={{ maxWidth: "370px" }}
          value={selectedLawType}
          onChange={(e) => {
            setSelectedLawType(e.target.value);
            loadLaws(e.target.value);
          }}
        >
          {lawTypes.map((value, index) => (
            <option key={index} value={value.value}>
              {getLocalByVal(value.text, value.textLocal)}
            </option>
          ))}
        </Form.Select>
        <Button
          type="submit"
          style={{ margin: "0" }}
          variant="outline-secondary"
          id="button-addon2"
        >
          <FontAwesomeIcon icon={faSearch} />
        </Button>
      </InputGroup>
    </Form>
  );
}
