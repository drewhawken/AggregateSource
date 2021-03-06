﻿using System;
using NUnit.Framework;

namespace AggregateSource.Testing
{
    namespace ThenStateBuilderTests
    {
        [TestFixture]
        public class WhenBuilderThenEventsTests : ThenEventsFixture
        {
            protected override IThenStateBuilder Then(string identifier, params object[] events)
            {
                return new Scenario().When(new object()).Then(identifier, events);
            }
        }

        [TestFixture]
        public class ThenEventsBuilderThenEventsTests : ThenEventsFixture
        {
            protected override IThenStateBuilder Then(string identifier, params object[] events)
            {
                return new Scenario().When(new object()).Then("", new object[0]).Then(identifier, events);
            }
        }

        public abstract class ThenEventsFixture
        {
            protected abstract IThenStateBuilder Then(string identifier, params object[] events);

            [Test]
            public void ThenThrowsWhenIdentifierIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => Then(null, new object[0]));
            }

            [Test]
            public void ThenThrowsWhenEventsAreNull()
            {
                Assert.Throws<ArgumentNullException>(() => Then(Model.Identifier1, null));
            }

            [Test]
            public void ThenDoesNotReturnNull()
            {
                var result = Then(Model.Identifier1, new object[0]);
                Assert.That(result, Is.Not.Null);
            }

            [Test]
            public void ThenReturnsThenBuilderContinuation()
            {
                var result = Then(Model.Identifier1, new object[0]);
                Assert.That(result, Is.InstanceOf<IThenStateBuilder>());
            }

            [Test]
            [Repeat(2)]
            public void ThenReturnsNewInstanceUponEachCall()
            {
                Assert.That(
                    Then(Model.Identifier1, new object[0]),
                    Is.Not.SameAs(Then(Model.Identifier1, new object[0])));
            }

            [Test]
            public void IsSetInResultingSpecification()
            {
                var events = new[] {new object(), new object()};

                var result = Then(Model.Identifier1, events).Build().Thens;

                Assert.That(result, Is.EquivalentTo(
                    new[]
                    {
                        new Tuple<string, object>(Model.Identifier1, events[0]),
                        new Tuple<string, object>(Model.Identifier1, events[1])
                    }));
            }
        }

        [TestFixture]
        public class WhenBuilderThenFactsTests : ThenFactsFixture
        {
            protected override IThenStateBuilder Then(params Tuple<string, object>[] facts)
            {
                return new Scenario().When(new object()).Then(facts);
            }
        }

        [TestFixture]
        public class ThenFactsBuilderThenFactsTests : ThenFactsFixture
        {
            protected override IThenStateBuilder Then(params Tuple<string, object>[] facts)
            {
                return new Scenario().When(new object()).Then("", new object[0]).Then(facts);
            }
        }

        public abstract class ThenFactsFixture
        {
            protected abstract IThenStateBuilder Then(params Tuple<string, object>[] facts);

            Tuple<string, object> _fact;

            [SetUp]
            public void SetUp()
            {
                _fact = new Tuple<string, object>(Model.Identifier1, new object());
            }

            [Test]
            public void ThenThrowsWhenFactsAreNull()
            {
                Assert.Throws<ArgumentNullException>(() => Then(null));
            }

            [Test]
            public void ThenDoesNotReturnNull()
            {
                var result = Then(_fact);
                Assert.That(result, Is.Not.Null);
            }

            [Test]
            public void ThenReturnsThenBuilderContinuation()
            {
                var result = Then(_fact);
                Assert.That(result, Is.InstanceOf<IThenStateBuilder>());
            }

            [Test]
            [Repeat(2)]
            public void ThenReturnsNewInstanceUponEachCall()
            {
                Assert.That(
                    Then(_fact),
                    Is.Not.SameAs(Then(_fact)));
            }

            [Test]
            public void IsSetInResultingSpecification()
            {
                var facts = new[]
                {
                    new Tuple<string, object>(Model.Identifier1, new object()),
                    new Tuple<string, object>(Model.Identifier2, new object()),
                };

                var result = Then(facts).Build().Thens;

                Assert.That(result, Is.EquivalentTo(facts));
            }
        }
    }
}