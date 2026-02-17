import React from "react";
import { Button, Col, FormControl, InputGroup, Row } from "react-bootstrap";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function SearchForm({ searchSubmit, setSearchKey, searchKey }) {
  return (
    <Row>
      <Col sm={12} md={10} lg={9}>
        <div class="well">
          <form onSubmit={searchSubmit}>
            <InputGroup>
              <InputGroup.Text>Name</InputGroup.Text>
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
