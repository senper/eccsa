import React from "react";
import {
  Button,
  Col,
  Form,
  FormControl,
  InputGroup,
  Row,
} from "react-bootstrap";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function MMSearchForm({
  searchSubmit,
  selectedBusinessType,
  setSelectedBusinessType,
  setSearchKey,
  searchKey,
  businessTypes,
  getLocal,
  getLocalByVal,
}) {
  return (
    <Row>
      <Col sm={12} md={10} lg={9}>
        <div className="well">
          <form onSubmit={searchSubmit}>
            <InputGroup>
              <Form.Select
                md={1}
                style={{ maxWidth: "160px" }}
                value={selectedBusinessType ? selectedBusinessType : undefined}
                onChange={(e) => setSelectedBusinessType(e.target.value)}
              >
                {businessTypes.map((value, index) => (
                  <option key={index} value={value.id}>{getLocal(value.name)}</option>
                ))}
              </Form.Select>
              <FormControl
                type="text"
                aria-describedby="basic-addon2"
                value={searchKey}
                onChange={(e) => setSearchKey(e.target.value)}
              />
              <Button
                type="submit"
                style={{ margin: "0" }}
                variant="outline-secondary"
                id="button-addon2"
              >
                <FontAwesomeIcon icon={faSearch} />
              </Button>
            </InputGroup>
          </form>
        </div>
      </Col>
    </Row>
  );
}
