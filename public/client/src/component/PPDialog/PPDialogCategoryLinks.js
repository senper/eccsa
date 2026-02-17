import React from "react";

import { Link } from "react-router-dom";

import { Card } from "react-bootstrap";
import { getDialogList } from "../../services/resources";

export default function DialogCategoryLinks() {
  const [dialogCategories, setDialogCategories] = React.useState([]);
  React.useEffect(() => {
    getDialogList().then((data) => {
      setDialogCategories(data.data.$values);
    });
  }, []);
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header style={{textAlign: "center"}}>Issues</Card.Header>
      <Card.Body>
        <Card.Text>
          {dialogCategories.map((dialog) => (
            <li
              title={dialog.title}
              style={{
                whiteSpace: "nowrap",
                overflow: "hidden",
                maxWidth: "270px",
              }}
            >
              <Link
                title={dialog.description}
                to={{
                  pathname: "/ppDialog",
                  state: {
                    categoryId: dialog.id,
                  },
                }}
                eventKey="ppDialog"
              >
                {dialog.title}
              </Link>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
